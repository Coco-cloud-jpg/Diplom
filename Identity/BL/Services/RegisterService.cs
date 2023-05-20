using BL.Services.Interfaces;
using Common.Emails;
using Common.Models;
using Common.Services.Interfaces;
using DAL.DTO;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Services
{
    public class RegisterService: IRegisterService
    {
        private readonly ICryptoService _cryptoService;
        private readonly IEmailSender _emailSender;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        public RegisterService(IIdentityUnitOfWork identityUnitOfWork, ICryptoService cryptoService, IEmailSender emailSender)
        {
            _identityUnitOfWork = identityUnitOfWork;
            _cryptoService = cryptoService;
            _emailSender = emailSender;
        }
        public async Task Register(UserCreateDTO model)
        {
            if (await _identityUnitOfWork.UserRepository.DbSet.FirstOrDefaultAsync(item => item.Email == model.Email) != null)
                throw new ArgumentNullException("This email already exists in system");

            await _identityUnitOfWork.UserRepository.Create(new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = _cryptoService.ComputeSHA256(model.Password),
                RoleId = model.RoleId,
                CompanyId = model.CompanyId,
                IsActive = true
            }, CancellationToken.None);

            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        public async Task Register(AdminCreate model)
        {
            if (await _identityUnitOfWork.UserRepository.DbSet.FirstOrDefaultAsync(item => item.Email == model.Email) != null)
                throw new ArgumentNullException("This email already exists in system");

            var relatedCompany = await _identityUnitOfWork.CompanyRepository.GetById(model.CompanyId, CancellationToken.None);

            if (relatedCompany == null || relatedCompany.Email != model.Email)
                throw new ArgumentNullException("Cannot create this user as company admin");

            await _identityUnitOfWork.UserRepository.Create(new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = _cryptoService.ComputeSHA256(model.Password),
                RoleId = Guid.Parse(Common.Constants.Role.CompanyAdmin),
                CompanyId = model.CompanyId,
                IsActive = true
            }, CancellationToken.None);

            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        public async Task<bool> CheckEmail(string email) =>
            await IsEmailAlreadyExists(email);
        public async Task<string> GetEmail(Guid companyId)
        {
            var company = await _identityUnitOfWork.CompanyRepository.GetById(companyId, CancellationToken.None);

            if (company == null)
                throw new ArgumentNullException();

            return company.Email;
        }
        public async Task RegisterCompany(string refererUrl, CompanyCreate model)
        {
            if (await IsEmailAlreadyExists(model.Email))
                throw new ArgumentNullException("This email already exists in system");

            var createdId = Guid.NewGuid();

            var utcNow = DateTime.UtcNow;

            await _identityUnitOfWork.CompanyRepository.Create(new Company
            {
                Id = createdId,
                Name = model.Name,
                Email = model.Email,
                IsActive = true,
                DateCreated = utcNow,
                CountryId = model.CountryId,
                TimeToPayForBills = utcNow.AddDays(2)
            }, CancellationToken.None);

            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);

            var template = System.IO.File.ReadAllText("../BL/Templates/Email_User_Register.html");

            template = template.Replace("{{url}}", $"{refererUrl}admin-register/{createdId}");

            var message = new Message(new string[] { model.Email }, "Create company admin!", template);
            _emailSender.SendEmail(message);
        }
        public async Task<IEnumerable<ShortEntityModel<Guid>>> GetCountries(CancellationToken cancellationToken) =>
            (await _identityUnitOfWork.CountryRepository.GetAll(cancellationToken)).OrderBy(item => item.Name).Select(item => new ShortEntityModel<Guid> { Id = item.Id, Name = item.Name });

        public async Task PasswordResetRequest(string refererUrl, ResetPasswordRequest model)
        {
            var user = await _identityUnitOfWork.UserRepository.DbSet.Include(item => item.PasswordReset)
                .FirstOrDefaultAsync(item => item.Email == model.Email);

            if (user == null)
                throw new ArgumentNullException("User with this email doesn't exist in system");

            Guid requestId;

            if (user.PasswordReset != null)
            {
                requestId = user.PasswordReset.Id;
            }
            else
            {
                requestId = Guid.NewGuid();

                await _identityUnitOfWork.PasswordResetRepository.Create(new PasswordReset
                {
                    Id = requestId,
                    UserId = user.Id,
                }, CancellationToken.None);

                await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);
            }

            var template = System.IO.File.ReadAllText("../BL/Templates/Email_User_PasswordReset.html");

            template = template.Replace("{{url}}", $"{refererUrl}password-reset/{requestId}");

            var message = new Message(new string[] { model.Email }, "Reset your password!", template);
            _emailSender.SendEmail(message);
        }
        public async Task ResetPassword(ResetPasswordSubmit model)
        {
            var request = await _identityUnitOfWork.PasswordResetRepository.DbSet.Include(item => item.User)
                .FirstOrDefaultAsync(item => item.Id == model.RequestId);

            if (request == null)
                throw new ArgumentNullException();

            request.User.Password = _cryptoService.ComputeSHA256(model.Password);

            await _identityUnitOfWork.PasswordResetRepository.Delete(model.RequestId, CancellationToken.None);

            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        private async Task<bool> IsEmailAlreadyExists(string email)
        {
            return await _identityUnitOfWork.CompanyRepository.DbSet.FirstOrDefaultAsync(item => item.Email == email) != null ||
                await _identityUnitOfWork.UserRepository.DbSet.FirstOrDefaultAsync(item => item.Email == email) != null;
        }
    }
}
