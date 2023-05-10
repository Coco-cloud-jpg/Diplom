using Identity.DTO;
using Identity.Interfaces;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.Constants;
using Common.Extensions;
using Common.Models;
using Common.Emails;
using Common.Services.Interfaces;
using System.Security.Claims;

namespace Identity.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        private IIdentityUnitOfWork _unitOfWork;
        private IEmailSender _emailSender;
        public UsersController(IIdentityUnitOfWork unitOfWork, IEmailSender emailSender, ICryptoService cryptoService)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _cryptoService = cryptoService;
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> GetAllUsers(bool includeDeleted, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                var data = await _unitOfWork.UserRepository.DbSet
                    .Include(item => item.Role)
                    .AsNoTracking()
                    .Where(item => item.CompanyId == companyId && (includeDeleted ? true : item.IsActive))
                    .Select(item => new UserReadDTO
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        Role = item.Role.Name,
                        IsActive = item.IsActive
                    })
                    .ToListAsync(cancellationToken);

                /*var count = new Random();

                data = new List<UserReadDTO>(data);

                for (int i = 0; i < 50; i++)
                {
                    data.Add(new UserReadDTO { Id= Guid.NewGuid(), FirstName = $"{count.Next(0, 150)}" });
                }
                */
                return Ok(data);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpPatch("status-toggle/{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var userIdToPerformAction = Guid.Parse(this.GetClaim(ClaimTypes.NameIdentifier));

            var userToDisable = await _unitOfWork.UserRepository.GetById(id, cancellationToken);

            if (userToDisable.Password == String.Empty)
                return BadRequest("User was never activated!");

            userToDisable.IsActive = !userToDisable.IsActive;
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return Ok(new { ToLogout = userIdToPerformAction == id});
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> Create(UserUpdateDTO model, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.UserRepository.DbSet.FirstOrDefaultAsync(item => item.Email == model.Email) != null)
                return BadRequest("This email already exists in system!");

            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            if (!await HasCompanyFreePlaces(companyId))
                return BadRequest("Your plan doesn't support new users!");

            var createdId = Guid.NewGuid();

            var dbModel = new User
            {
                Id = createdId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActive = false,
                RoleId = model.Role,
                CompanyId = companyId,
                Password = String.Empty
            };

            await _unitOfWork.UserRepository.Create(dbModel, cancellationToken);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            var refererUrl = this.HttpContext.Request.Headers["Referer"].ToString();

            var template = System.IO.File.ReadAllText("Templates/Email_User_Adding.html");

            template = template.Replace("{{url}}", $"{refererUrl}user-register/{companyId}/{createdId}");

            var message = new Message(new string[] { model.Email }, "User Registration!", template);
            _emailSender.SendEmail(message);

            return Ok();
        }
        [HttpPost("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> Submit(SubmitUserRegisration model, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _unitOfWork.UserRepository.GetById(model.UserId, cancellationToken);
            
            if (itemToUpdate.CompanyId != model.CompanyId)
                return BadRequest();

            itemToUpdate.Password = _cryptoService.ComputeSHA256(model.Password);
            itemToUpdate.IsActive = true;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        [HttpGet("email/{companyId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmail(Guid companyId, Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetById(userId, cancellationToken);

                if ((user.CompanyId != companyId) || (user.Password != String.Empty))
                    return BadRequest();

                return Ok(user.Email);
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> Update(Guid id, UserUpdateDTO model, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _unitOfWork.UserRepository.GetById(id, cancellationToken);

            if (itemToUpdate == null)
                return NotFound();

            itemToUpdate.FirstName = model.FirstName;
            itemToUpdate.LastName = model.LastName;
            itemToUpdate.Email = model.Email;
            itemToUpdate.RoleId = model.Role;
            
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return Ok();
        }
        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken) =>
            Ok(await _unitOfWork.RoleRepository.DbSet.AsNoTracking()
                .Where(item => item.Id != Guid.Parse(Common.Constants.Role.SystemAdmin)).ToListAsync(cancellationToken));

        private async Task<bool> HasCompanyFreePlaces(Guid companyId)
        {
            var maxUsersCount = await _unitOfWork.PackageTypeCompanyRepository.DbSet.Include(item => item.PackageType)
                .AsNoTracking().Where(item => item.CompanyId == companyId).SumAsync(item => (int)(item.Count * item.PackageType.MaxUsersCount));

            var currentUsersCount = await _unitOfWork.UserRepository.DbSet.AsNoTracking().Where(item => item.CompanyId == companyId).CountAsync();
        
            return maxUsersCount > currentUsersCount;
        }
    }
}
