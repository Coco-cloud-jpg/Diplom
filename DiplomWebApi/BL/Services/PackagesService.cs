using Common.Models;
using DAL.DTOS;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;

namespace BL.Services
{
    public class PackagesService: IPackagesService
    {
        private IScreenUnitOfWork _unitOfWork;
        public PackagesService(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PackageTypeReadDTO>> GetAllPackages(CancellationToken cancellationToken) =>
            (await _unitOfWork.PackageTypeRepository.GetAll(cancellationToken)).OrderBy(item => item.Price)
                .Select(item => new PackageTypeReadDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    MaxRecordersCount = item.MaxRecordersCount,
                    MaxUsersCount = item.MaxUsersCount,
                    Price = item.Price,
                    Currency = ((Currency)item.Currency).ToString(),
                    CurrencyShort = item.Currency
                }).ToList();
        public async Task<Guid> Create(PackageTypeDTO model, CancellationToken cancellationToken)
        {
            var packageType = new PackageType
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                MaxUsersCount = model.MaxUsersCount,
                MaxRecordersCount = model.MaxRecordersCount,
                Price = model.Price,
                Currency = model.CurrencyShort
            };

            await _unitOfWork.PackageTypeRepository.Create(packageType, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return packageType.Id;
        }
        public async Task<Guid> Update(Guid id, PackageTypeDTO model, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _unitOfWork.PackageTypeRepository.GetById(id, cancellationToken);

            itemToUpdate.Name = model.Name;
            itemToUpdate.MaxUsersCount = model.MaxUsersCount;
            itemToUpdate.MaxRecordersCount = model.MaxRecordersCount;
            itemToUpdate.Price = model.Price;
            itemToUpdate.Currency = model.CurrencyShort;

            _unitOfWork.PackageTypeRepository.Update(itemToUpdate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return itemToUpdate.Id;
        }
        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.PackageTypeCompanyRepository.DbSet.AnyAsync(item => item.PackageTypeId == id))
                throw new Exception("Cannot delete package type. Some companies use it.");

            await _unitOfWork.PackageTypeRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<PackageTypeCompaniesReadDTO>> GetCompanies(Guid id, CancellationToken cancellationToken) =>
            await _unitOfWork.PackageTypeCompanyRepository.DbSet.Include(item => item.Company)
                .Where(item => item.PackageTypeId == id)
                .Select(item => new PackageTypeCompaniesReadDTO
                {
                    CompanyId = item.CompanyId,
                    CompanyName = item.Company.Name,
                    Count = item.Count
                }).ToListAsync(cancellationToken);
        public IEnumerable<ShortEntityModelShort> GetCurrencies() =>
            Enum.GetValues(typeof(Currency)).Cast<Currency>()
                .Select(item => new ShortEntityModelShort 
                {
                    Id = (short)item,
                    Name = item.ToString()
                });
    }
}
