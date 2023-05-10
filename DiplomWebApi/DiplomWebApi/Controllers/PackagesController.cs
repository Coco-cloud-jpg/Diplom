using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;
using ScreenMonitorService.Interfaces;

namespace DiplomWebApi.Controllers
{
    [Route("api/packages")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        public PackagesController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /*[HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetAllPackages(CancellationToken cancellationToken)
        {
            var data = (await _unitOfWork.PackageTypeRepository.GetAll(cancellationToken)).OrderBy(item => item.Price)
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
            data.AddRange(data);
            data.AddRange(data);
            data.AddRange(data);
            return Ok(data);
        }*/
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetAllPackages(CancellationToken cancellationToken) =>
            Ok((await _unitOfWork.PackageTypeRepository.GetAll(cancellationToken)).OrderBy(item => item.Price)
                .Select(item => new PackageTypeReadDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    MaxRecordersCount = item.MaxRecordersCount,
                    MaxUsersCount = item.MaxUsersCount,
                    Price = item.Price,
                    Currency = ((Currency)item.Currency).ToString(),
                    CurrencyShort = item.Currency
                }));
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Create(PackageTypeDTO model, CancellationToken cancellationToken)
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

            return Ok(packageType.Id);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Update(Guid id, PackageTypeDTO model, CancellationToken cancellationToken)
        {
            try
            {
                var itemToUpdate = await _unitOfWork.PackageTypeRepository.GetById(id, cancellationToken);
                
                itemToUpdate.Name = model.Name;
                itemToUpdate.MaxUsersCount = model.MaxUsersCount;
                itemToUpdate.MaxRecordersCount = model.MaxRecordersCount;
                itemToUpdate.Price = model.Price;
                itemToUpdate.Currency = model.CurrencyShort;

                _unitOfWork.PackageTypeRepository.Update(itemToUpdate);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Ok(itemToUpdate.Id);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.PackageTypeCompanyRepository.DbSet.AnyAsync(item => item.PackageTypeId == id))
                return BadRequest("Cannot delete package type. Some companies use it.");

            await _unitOfWork.PackageTypeRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        [HttpGet("{id}/companies")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetCompanies(Guid id, CancellationToken cancellationToken) =>
            Ok(await _unitOfWork.PackageTypeCompanyRepository.DbSet.Include(item => item.Company)
                .Where(item => item.PackageTypeId == id)
                .Select(item => new PackageTypeCompaniesReadDTO
                {
                    CompanyId = item.CompanyId,
                    CompanyName = item.Company.Name,
                    Count = item.Count
                }).ToListAsync(cancellationToken));

        [HttpGet("currency")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public IActionResult GetCurrencies() =>
            Ok(Enum.GetValues(typeof(Currency)).Cast<Currency>()
                .Select(item => new { 
                    Id = (short)item, 
                    Name = item.ToString()
                }));
    }
}
