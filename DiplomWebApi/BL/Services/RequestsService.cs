using Common.Models;
using DAL.DTOS;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;

namespace BL.Services
{
    public class RequestsService: IRequestsService
    {
        private IScreenUnitOfWork _unitOfWork;
        public RequestsService(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RequestReadDTO>> GetCompanyRequests(Guid id, CancellationToken cancellationToken) =>
            await _unitOfWork.PackageUpgradeRequestRepository.DbSet.AsNoTracking()
                .Include(item => item.PackageType)
                .Where(item => item.CompanyId == id && item.Status == (short)RequestStatus.Pending)
                .Select(item => new RequestReadDTO
                {
                    Id = item.Id,
                    PackagesCount = item.PackagesCount,
                    TimePosted = item.TimePosted,
                    PackageType = item.PackageType.Name
                }).ToListAsync(cancellationToken);
        public async Task Create(Guid id, CancellationToken cancellationToken)
        {
            var requestToReject = await _unitOfWork.PackageUpgradeRequestRepository.GetById(id, cancellationToken);
            requestToReject.Status = (short)RequestStatus.Pending;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task Reject(Guid id, CancellationToken cancellationToken)
        {
            var requestToReject = await _unitOfWork.PackageUpgradeRequestRepository.GetById(id, cancellationToken);
            requestToReject.Status = (short)RequestStatus.Rejected;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task Approve(Guid id, CancellationToken cancellationToken)
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
        }
    }
}
