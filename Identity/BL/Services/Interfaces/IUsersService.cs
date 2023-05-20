using Common.Models;
using DAL.DTO;

namespace BL.Services
{
    public interface IUsersService
    {
        Task<List<UserReadDTO>> GetAllUsers(Guid companyId, bool includeDeleted, CancellationToken cancellationToken);
        Task<bool> ToggleStatus(Guid userIdToPerformAction, Guid id, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
        Task Create(Guid companyId, string refererUrl, UserUpdateDTO model, CancellationToken cancellationToken);
        Task Submit(SubmitUserRegisration model, CancellationToken cancellationToken);
        Task<string> GetEmail(Guid companyId, Guid userId, CancellationToken cancellationToken);
        Task Update(Guid id, UserUpdateDTO model, CancellationToken cancellationToken);
        Task<List<Role>> GetRoles(CancellationToken cancellationToken);
    }
}
