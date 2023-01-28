using Common.Emails;
using Common.Models;
using Common.Services.Interfaces;
using Identity.DTO;
using Identity.Interfaces;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.Security.Claims;

namespace Identity.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        private readonly IEmailSender _emailSender;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        public RegisterController(IIdentityUnitOfWork identityUnitOfWork, ICryptoService cryptoService, IEmailSender emailSender)
        {
            _identityUnitOfWork = identityUnitOfWork;
            _cryptoService = cryptoService;
            _emailSender = emailSender;
        }
        [HttpPost("user")]
        [Authorize(Roles = nameof(Common.Constants.Role.SystemAdmin))]
        public async Task<ActionResult> Register(UserCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad credentials");

            if (await _identityUnitOfWork.UserRepository.DbSet.FirstOrDefaultAsync(item => item.Email == model.Email) != null)
                return BadRequest("This email already exists in system");

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

            return Ok();
        }
        [HttpPost("admin")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(AdminCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            if (await _identityUnitOfWork.UserRepository.DbSet.FirstOrDefaultAsync(item => item.Email == model.Email) != null)
                return BadRequest("This email already exists in system");

            var relatedCompany = await _identityUnitOfWork.CompanyRepository.GetById(model.CompanyId, CancellationToken.None);

            if (relatedCompany == null || relatedCompany.Email != model.Email)
                return BadRequest("Cannot create this user as company admin");

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

            return Ok();
        }
        [HttpGet("email/exists/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult> CheckEmail(string email)
        {
            return Ok(await IsEmailAlreadyExists(email));
        }
        [HttpGet("email/{companyId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetEmail(Guid companyId)
        {
            var company = await _identityUnitOfWork.CompanyRepository.GetById(companyId, CancellationToken.None);

            if (company == null)
                return NotFound();

            return Ok(company.Email);
        }
        [HttpPost("company")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterCompany(CompanyCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            if (await IsEmailAlreadyExists(model.Email))
                return BadRequest("This email already exists in system");

            var createdId = Guid.NewGuid();

            await _identityUnitOfWork.CompanyRepository.Create(new Company
            {
                Id = createdId,
                Name = model.Name,
                Email = model.Email,
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                CountryId = model.CountryId
            }, CancellationToken.None);

            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);

            var refererUrl = this.HttpContext.Request.Headers["Referer"].ToString();

            var template = System.IO.File.ReadAllText("Templates/Email_User_Register.html");

            template = template.Replace("{{url}}", $"{refererUrl}admin-register/{createdId}");

            var message = new Message(new string[] { model.Email }, "Create company admin!", template);
            _emailSender.SendEmail(message);

            return Ok();
        }
        [HttpGet("countries")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCountries(CancellationToken cancellationToken)
        {
            return Ok((await _identityUnitOfWork.CountryRepository.GetAll(cancellationToken)).OrderBy(item => item.Name).Select(item => new { Id = item.Id, Name = item.Name}));
        }
        [HttpPost("password-reset-request")]
        [AllowAnonymous]
        public async Task<ActionResult> PasswordResetRequest(ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            var user = await _identityUnitOfWork.UserRepository.DbSet.Include(item => item.PasswordReset)
                .FirstOrDefaultAsync(item => item.Email == model.Email);

            if (user == null)
                return BadRequest("User with this email doesn't exist in system");

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

            var refererUrl = this.HttpContext.Request.Headers["Referer"].ToString();

            var template = System.IO.File.ReadAllText("Templates/Email_User_PasswordReset.html");

            template = template.Replace("{{url}}", $"{refererUrl}password-reset/{requestId}");

            var message = new Message(new string[] { model.Email }, "Reset your password!", template);
            _emailSender.SendEmail(message);

            return Ok();
        }
        [HttpPost("password-reset")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordSubmit model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid model");

                var request = await _identityUnitOfWork.PasswordResetRepository.DbSet.Include(item => item.User)
                    .FirstOrDefaultAsync(item => item.Id == model.RequestId);

                if (request == null)
                    return NotFound();

                request.User.Password = _cryptoService.ComputeSHA256(model.Password);

                await _identityUnitOfWork.PasswordResetRepository.Delete(model.RequestId, CancellationToken.None);

                await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);

                return Ok();
            }
            catch (Exception e)
            {
                var s = 2;
                throw;
            }
        }
        private async Task<bool> IsEmailAlreadyExists(string email)
        {
            return await _identityUnitOfWork.CompanyRepository.DbSet.FirstOrDefaultAsync(item => item.Email == email) != null || 
                await _identityUnitOfWork.UserRepository.DbSet.FirstOrDefaultAsync(item => item.Email == email) != null;
        }
    }
}
