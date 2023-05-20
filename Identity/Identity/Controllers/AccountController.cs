using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.Extensions;
using System.Security.Claims;
using BL.Services;

namespace Identity.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var roleId = this.GetClaim("RoleId");
            var userId = Guid.Parse(this.GetClaim(ClaimTypes.NameIdentifier));

            if (roleId == null)
                return Unauthorized();

            return Ok(await _accountService.Get(roleId, userId));
        }
    }
}
