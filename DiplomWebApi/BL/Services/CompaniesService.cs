using Common.Models;
using DAL.DTOS;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CompaniesService: ICompaniesService
    {
        private IScreenUnitOfWork _unitOfWork;
        public CompaniesService(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CompanyDTO>> GetAllCompanies(bool includeDeleted, CancellationToken cancellationToken) =>
            await _unitOfWork.CompanyRepository.DbSet
                    .Include(item => item.Country)
                    .Where(item => includeDeleted ? true : item.IsActive)
                    .Select(item => new CompanyDTO
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Email = item.Email,
                        IsActive = item.IsActive,
                        DateCreated = item.DateCreated,
                        Country = item.Country.Name
                    })
                    .ToListAsync(cancellationToken);
        public async Task ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var itemToDisable = await _unitOfWork.CompanyRepository.GetById(id, cancellationToken);

            itemToDisable.IsActive = !itemToDisable.IsActive;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<CompanyDTO> GetCompanyInfo(Guid id, CancellationToken cancellationToken) =>
            await _unitOfWork.CompanyRepository.DbSet.Include(item => item.Country)
                                .AsNoTracking().Where(item => item.Id == id)
                                .Select(item => new CompanyDTO
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Email = item.Email,
                                    IsActive = item.IsActive,
                                    DateCreated = item.DateCreated,
                                    Country = item.Country.Name,
                                    TimeToPay = item.TimeToPayForBills
                                }).FirstOrDefaultAsync(cancellationToken);

        //[Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<CompanyBillingResponse> GetCompanysPackages(Guid id, CancellationToken cancellationToken)
        {
            var packages = await _unitOfWork.PackageTypeCompanyRepository.DbSet.Include(item => item.PackageType)
                .AsNoTracking().Where(item => item.CompanyId == id)
                .Select(item => new CompanysPackagesDTO
                {
                    Name = item.PackageType.Name,
                    Total = item.Count,
                    Price = item.Count * item.PackageType.Price,
                    MaxRecordersCount = item.PackageType.MaxRecordersCount * item.Count,
                    MaxUsersCount = item.PackageType.MaxUsersCount * item.Count,
                    Currency = (Currency)item.PackageType.Currency
                }).ToListAsync(cancellationToken);

            var maxUsersCount = packages.Sum(item => item.MaxUsersCount);
            var maxRecordersCount = packages.Sum(item => item.MaxRecordersCount);

            var currentCounts = (await _unitOfWork.CompanyUsersAndRecordersCountDTORepository
                .DbSet.FromSqlRaw(@$"SELECT Id, (select count(1) from Users where companyId = c.id and IsActive = 1) as UsersCount,
                                                (select count(1) from RecorderRegistrations where companyId = c.id and IsActive = 1) as recordersCount FROM COMPANIES as c WHERE ID = '{id}'")
                .ToListAsync()).FirstOrDefault();

            var totalDollarsToPay = packages.Where(item => item.Currency == Currency.USD).Sum(item => item.Price);
            var totalEurosToPay = packages.Where(item => item.Currency == Currency.EUR).Sum(item => item.Price);
            var totalUAHToPay = packages.Where(item => item.Currency == Currency.UAH).Sum(item => item.Price);

            return new CompanyBillingResponse
            {
                MaxRecordersCount = maxRecordersCount,
                MaxUsersCount = maxUsersCount,
                Packages = packages,
                UsersCount = currentCounts.UsersCount,
                RecordersCount = currentCounts.RecordersCount,
                MonthlyDollarsCharge = totalDollarsToPay,
                MonthlyEurosCharge = totalEurosToPay,
                MonthlyUAHCharge = totalUAHToPay
            };
        }
    }
}
