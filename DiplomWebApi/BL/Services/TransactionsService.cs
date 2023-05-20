using Common.Models;
using DAL.DTOS;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Services
{
    public class TransactionsService: ITransactionsService
    {
        private IScreenUnitOfWork _unitOfWork;
        public TransactionsService(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<TransactionReadDTO>> GetAllTransactions(Guid id, CancellationToken cancellationToken) =>
            await _unitOfWork.BillingTransactionRepository.DbSet.AsNoTracking().
                Where(item => item.CompanyId == id).OrderByDescending(item => item.PaymentDate)
                .Select(item => new TransactionReadDTO
                {
                    Id = item.Id,
                    Currency = item.Currency,
                    PaymentDate = item.PaymentDate,
                    Sum = item.AmountPaid
                }).ToListAsync(cancellationToken);

        public async Task<Guid> Create(TransactionCreateDTO model, CancellationToken cancellationToken)
        {
            var transaction = new BillingTransaction
            {
                Id = Guid.NewGuid(),
                PaymentDate = DateTime.UtcNow,
                AmountPaid = model.Sum,
                Currency = model.Currency,
                CompanyId = model.CompanyId
            };

            await _unitOfWork.BillingTransactionRepository.Create(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            var company = await _unitOfWork.CompanyRepository.GetById(model.CompanyId, CancellationToken.None);

            var transactionAfterNotification = (await _unitOfWork.BillingTransactionRepository.DbSet.AsNoTracking()
                .Where(item => item.PaymentDate > company.TimeToPayForBills && item.CompanyId == model.CompanyId).ToListAsync(cancellationToken)).GroupBy(item => item.Currency).Select(item => new BillingByCurrency
                {
                    Currency = item.Select(b => b.Currency).FirstOrDefault(),
                    Sum = item.Sum(b => b.AmountPaid)
                }).ToList();

            bool wasPaidFully = true;

            var sumsToPayByPlans = await GetMonthlyPayingRate(model.CompanyId);

            sumsToPayByPlans.ForEach(item =>
            {
                if (wasPaidFully)
                    wasPaidFully = item.Sum <= transactionAfterNotification.FirstOrDefault(t => t.Currency == item.Currency)?.Sum;
            });

            if (wasPaidFully)
            {
                company.TimeToPayForBills = company.TimeToPayForBills.AddMonths(1);
                await _unitOfWork.SaveChangesAsync(CancellationToken.None);
            }

            return transaction.Id;
        }

        private async Task<List<BillingByCurrency>> GetMonthlyPayingRate(Guid companyId)
        {
            var sumToPayByCurrencies = await _unitOfWork.PackageTypeCompanyRepository.DbSet.AsNoTracking().Include(item => item.PackageType)
                .Where(item => item.CompanyId == companyId).Select(item => new BillingByCurrency
                {
                    Currency = item.PackageType.Currency,
                    Sum = item.PackageType.Price * item.Count
                }).ToListAsync();

            return sumToPayByCurrencies.GroupBy(item => item.Currency).Select(item => new BillingByCurrency
            {
                Currency = item.Select(b => b.Currency).FirstOrDefault(),
                Sum = item.Sum(b => b.Sum)
            }).ToList();
        }
    }
}
