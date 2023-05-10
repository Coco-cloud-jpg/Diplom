using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;
using ScreenMonitorService.Interfaces;

namespace DiplomWebApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        public CompaniesController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetAllCompanies(bool includeDeleted, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _unitOfWork.CompanyRepository.DbSet
                    .Include(item => item.Country)
                    .Where(item => includeDeleted ? true: item.IsActive)
                    .Select(item => new CompanyDTO
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Email = item.Email,
                        IsActive = item.IsActive,
                        DateCreated = item.DateCreated,
                        Country = item.Country.Name
                    })
                    .ToListAsync(cancellationToken));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPatch("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var itemToDisable = await _unitOfWork.CompanyRepository.GetById(id, cancellationToken);

            itemToDisable.IsActive = !itemToDisable.IsActive;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetCompanyInfo(Guid id, CancellationToken cancellationToken) =>
            Ok(await _unitOfWork.CompanyRepository.DbSet.Include(item => item.Country)
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
                                }).FirstOrDefaultAsync(cancellationToken));

        [HttpGet("{id}/packages")]
        [AllowAnonymous]
        //[Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetCompanysPackages(Guid id, CancellationToken cancellationToken)
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

            return Ok(new CompanyBillingResponse
            {
                MaxRecordersCount = maxRecordersCount,
                MaxUsersCount = maxUsersCount,
                Packages = packages,
                UsersCount = currentCounts.UsersCount,
                RecordersCount = currentCounts.RecordersCount,
                MonthlyDollarsCharge = totalDollarsToPay,
                MonthlyEurosCharge = totalEurosToPay,
                MonthlyUAHCharge = totalUAHToPay
            });
        }
    }
}
