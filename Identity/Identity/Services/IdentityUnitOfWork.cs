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
        private IGenericRepository<Company> _companyRepository;
        private IGenericRepository<Country> _countryRepository;
        private IGenericRepository<RefreshToken> _refreshTokenRepository;
        private IGenericRepository<PasswordReset> _passwordResetRepository;
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
        public IGenericRepository<PasswordReset> PasswordResetRepository
        {
            get
            {
                if (this._passwordResetRepository == null)
                {
                    this._passwordResetRepository = new GenericRepository<PasswordReset>(_context);
                }
                return _passwordResetRepository;
            }
        }
        public IGenericRepository<Country> CountryRepository
        {
            get
            {
                if (this._countryRepository == null)
                {
                    this._countryRepository = new GenericRepository<Country>(_context);
                }
                return _countryRepository;
            }
        }
        public IGenericRepository<Company> CompanyRepository
        {
            get
            {
                if (this._companyRepository == null)
                {
                    this._companyRepository = new GenericRepository<Company>(_context);
                }
                return _companyRepository;
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
