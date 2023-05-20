using DAL.DTOS;

namespace BL.Services
{
    public interface IRequestsService
    {
        Task<List<RequestReadDTO>> GetCompanyRequests(Guid id, CancellationToken cancellationToken);
        Task Create(Guid id, CancellationToken cancellationToken);
        Task Reject(Guid id, CancellationToken cancellationToken);
        Task Approve(Guid id, CancellationToken cancellationToken);
    }
}
