using Common.Interfaces;
using Common.Models;
using Common.Models;

namespace DAL.Interfaces
{
    public interface IIdentityUnitOfWork
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<RefreshToken> RefreshTokenRepository { get; }
        IGenericRepository<Company> CompanyRepository { get; }
        IGenericRepository<Country> CountryRepository { get; }
        IGenericRepository<PasswordReset> PasswordResetRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<PackageType> PackageTypeRepository { get; }
        IGenericRepository<PackageTypeCompany> PackageTypeCompanyRepository { get; }
        Task SaveChangesAsync(CancellationToken cancel);
    }
}
