using Identity.DTO;
using Identity.Interfaces;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.Constants;
using Common.Extensions;
using System.Security.Claims;

namespace Identity.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IIdentityUnitOfWork _unitOfWork;

        private readonly Dictionary<string, List<string>> _rolesToRoutes = new Dictionary<string, List<string>>
        {
            { Role.User, new List<string> { "/home", "/recorders", "/recorder-info/:id", "/alerts", "/warnings", "/reports" } },
            { Role.CompanyAdmin, new List<string> { "/home", "/recorders", "/recorder-info/:id", "/alerts", "/warnings", "/reports", "/users" } },
            { Role.SystemAdmin, new List<string> { "/home-admin", "/companies", "/billing" } }
        };

        public AccountController(IIdentityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var roleId = this.GetClaim("RoleId");
            var userId = Guid.Parse(this.GetClaim(ClaimTypes.NameIdentifier));

            if (roleId == null)
                return Unauthorized();

            var userInfo = await _unitOfWork.UserRepository.DbSet.Include(item => item.Company)
                .Include(item => item.Role).AsNoTracking().Where(item => item.Id == userId)
                .Select(item => new UserInfo
                {
                    Id = item.Id,
                    FullName = $"{item.FirstName} {item.LastName}",
                    Email = item.Email,
                    CompanyName = item.Company.Name,
                    RoleName = item.Role.Name
                }).FirstOrDefaultAsync();

            return Ok(new AccountDTORead
            {
                Routes = _rolesToRoutes[roleId],
                UserInfo = userInfo
            });
        }
    }
}
