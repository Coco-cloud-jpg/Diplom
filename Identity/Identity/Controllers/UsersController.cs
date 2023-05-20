using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.Extensions;
using System.Security.Claims;
using BL.Services;
using DAL.DTO;

namespace Identity.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUsersService _userService;
        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> GetAllUsers(bool includeDeleted, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                return Ok(await _userService.GetAllUsers(companyId, includeDeleted, cancellationToken));
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

            var toLogout = await _userService.ToggleStatus(userIdToPerformAction, id, cancellationToken);

            return Ok(new { ToLogout = toLogout });
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _userService.Delete(id, cancellationToken);

            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> Create(UserUpdateDTO model, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));
                var refererUrl = this.HttpContext.Request.Headers["Referer"].ToString();

                await _userService.Create(companyId, refererUrl, model, cancellationToken);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> Submit(SubmitUserRegisration model, CancellationToken cancellationToken)
        {
            await _userService.Submit(model, cancellationToken);
            return Ok();
        }
        [HttpGet("email/{companyId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmail(Guid companyId, Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _userService.GetEmail(companyId, userId, cancellationToken));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> Update(Guid id, UserUpdateDTO model, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.Update(id, model, cancellationToken);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken) =>
            Ok(await _userService.GetRoles(cancellationToken));
    }
}
