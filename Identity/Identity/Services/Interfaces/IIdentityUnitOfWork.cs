using Common.Interfaces;
using Common.Models;
using Common.Models;

namespace Identity.Interfaces
{
    public interface IIdentityUnitOfWork
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<RefreshToken> RefreshTokenRepository { get; }
        Task SaveChangesAsync(CancellationToken cancel);
    }
}
