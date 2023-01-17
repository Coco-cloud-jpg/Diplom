using Common.Interfaces;
using Common.Models;
using Common.Repositories.Repository;
using Identity.Interfaces;
using Identity.Models;

namespace Identity.Repositories.Repository
{
    public class IdentityUnitOfWork : BaseUnitOfWork, IIdentityUnitOfWork
    {
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<RefreshToken> _refreshTokenRepository;
        public IdentityUnitOfWork(IdentityContext context)
            : base(context)
        {
        }
        public IGenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new GenericRepository<User>(_context);
                }
                return _userRepository;
            }
        }
        public IGenericRepository<RefreshToken> RefreshTokenRepository
        {
            get
            {
                if (this._refreshTokenRepository == null)
                {
                    this._refreshTokenRepository = new GenericRepository<RefreshToken>(_context);
                }
                return _refreshTokenRepository;
            }
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
