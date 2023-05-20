using Common.Constants;
using DAL.DTO;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Services
{
    public class AccountService: IAccountService
    {
        private IIdentityUnitOfWork _unitOfWork;

        private readonly Dictionary<string, List<string>> _rolesToRoutes = new Dictionary<string, List<string>>
        {
            { Role.User, new List<string> { "/home", "/recorders", "/recorder-info/:id", "/alerts", "/warnings", "/reports" } },
            { Role.CompanyAdmin, new List<string> { "/home", "/recorders", "/recorder-info/:id", "/alerts", "/warnings", "/reports", "/users" } },
            { Role.SystemAdmin, new List<string> { "/home-admin", "/companies", "/billing" } }
        };

        public AccountService(IIdentityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AccountDTORead> Get(string roleId, Guid userId) =>
            new AccountDTORead
            {
                Routes = _rolesToRoutes[roleId],
                UserInfo = await _unitOfWork.UserRepository.DbSet.Include(item => item.Company)
                .Include(item => item.Role).AsNoTracking().Where(item => item.Id == userId)
                .Select(item => new UserInfo
                {
                    Id = item.Id,
                    FullName = $"{item.FirstName} {item.LastName}",
                    Email = item.Email,
                    CompanyName = item.Company.Name,
                    RoleName = item.Role.Name
                }).FirstOrDefaultAsync()
            };
    }
}
