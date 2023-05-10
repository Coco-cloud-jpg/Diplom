using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;
using ScreenMonitorService.Interfaces;

namespace DiplomWebApi.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        public RequestsController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetCompanyRequests(Guid id, CancellationToken cancellationToken) =>
            Ok(await _unitOfWork.PackageUpgradeRequestRepository.DbSet.AsNoTracking()
                .Include(item => item.PackageType)
                .Where(item => item.CompanyId == id && item.Status == (short)RequestStatus.Pending)
                .Select(item => new RequestReadDTO
                {
                    Id = item.Id,
                    PackagesCount = item.PackagesCount,
                    TimePosted = item.TimePosted,
                    PackageType = item.PackageType.Name
                }).ToListAsync(cancellationToken));
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> Create(Guid id, CancellationToken cancellationToken)
        {
            var requestToReject = await _unitOfWork.PackageUpgradeRequestRepository.GetById(id, cancellationToken);
            requestToReject.Status = (short)RequestStatus.Rejected;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        [HttpPatch("reject/{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Reject(Guid id, CancellationToken cancellationToken)
        {
            var requestToReject = await _unitOfWork.PackageUpgradeRequestRepository.GetById(id, cancellationToken);
            requestToReject.Status = (short)RequestStatus.Rejected;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        [HttpPatch("approve{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
        {
            var request = await _unitOfWork.PackageUpgradeRequestRepository.GetById(id, cancellationToken);
            request.Status = (short)RequestStatus.Approved;

            var companyPackageToUpdate = await _unitOfWork.PackageTypeCompanyRepository.DbSet
                .Where(item => item.CompanyId == request.CompanyId && item.PackageTypeId == request.PackageTypeId).FirstOrDefaultAsync(cancellationToken);


            if (companyPackageToUpdate != null)
            {
                companyPackageToUpdate.Count += request.PackagesCount;
                companyPackageToUpdate.DateModified = DateTime.UtcNow;
            }
            else
            {
                await _unitOfWork.PackageTypeCompanyRepository.Create(new PackageTypeCompany
                {
                    Id = Guid.NewGuid(),
                    PackageTypeId = request.PackageTypeId,
                    CompanyId = request.CompanyId,
                    Count = request.PackagesCount,
                    DateModified = DateTime.UtcNow
                }, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok();
        }
    }
}
