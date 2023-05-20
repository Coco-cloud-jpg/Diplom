using Common.Interfaces;
using Common.Models;
using Common.Repositories.Repository;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories.Repository
{
    public class IdentityUnitOfWork : BaseUnitOfWork, IIdentityUnitOfWork
    {
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<Company> _companyRepository;
        private IGenericRepository<Country> _countryRepository;
        private IGenericRepository<RefreshToken> _refreshTokenRepository;
        private IGenericRepository<PasswordReset> _passwordResetRepository;
        private IGenericRepository<Role> _roleRepository;
        private IGenericRepository<PackageType> _packageTypeRepository;
        private IGenericRepository<PackageTypeCompany> _packageTypeCompanyRepository;
        public IdentityUnitOfWork(IdentityContext context)
            : base(context)
        {
        }
        public IGenericRepository<PackageTypeCompany> PackageTypeCompanyRepository
        {
            get
            {
                if (this._packageTypeCompanyRepository == null)
                {
                    this._packageTypeCompanyRepository = new GenericRepository<PackageTypeCompany>(_context);
                }
                return _packageTypeCompanyRepository;
            }
        }
        public IGenericRepository<PackageType> PackageTypeRepository
        {
            get
            {
                if (this._packageTypeRepository == null)
                {
                    this._packageTypeRepository = new GenericRepository<PackageType>(_context);
                }
                return _packageTypeRepository;
            }
        }
        public IGenericRepository<Role> RoleRepository
        {
            get
            {
                if (this._roleRepository == null)
                {
                    this._roleRepository = new GenericRepository<Role>(_context);
                }
                return _roleRepository;
            }
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
