using DAL.DTO;

namespace BL.Services
{
    public interface IAccountService
    {
        Task<AccountDTORead> Get(string roleId, Guid userId);
    }
}
