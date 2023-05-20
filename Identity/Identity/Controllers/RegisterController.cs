using BL.Services;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        [HttpPost("user")]
        [Authorize(Roles = nameof(Common.Constants.Role.SystemAdmin))]
        public async Task<ActionResult> Register(UserCreateDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Bad credentials");

                await _registerService.Register(model);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("admin")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(AdminCreate model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid model");

                await _registerService.Register(model);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("email/exists/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult> CheckEmail(string email) => Ok(await _registerService.CheckEmail(email));
        [HttpGet("email/{companyId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetEmail(Guid companyId)
        {
            try
            {
                return Ok(await _registerService.GetEmail(companyId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost("company")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterCompany(CompanyCreate model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid model");

                var refererUrl = this.HttpContext.Request.Headers["Referer"].ToString();

                await _registerService.RegisterCompany(refererUrl, model);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("countries")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCountries(CancellationToken cancellationToken) =>
            Ok(await _registerService.GetCountries(cancellationToken));
        [HttpPost("password-reset-request")]
        [AllowAnonymous]
        public async Task<ActionResult> PasswordResetRequest(ResetPasswordRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid model");

                var refererUrl = this.HttpContext.Request.Headers["Referer"].ToString();
                await _registerService.PasswordResetRequest(refererUrl, model);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpPost("password-reset")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordSubmit model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid model");

                await _registerService.ResetPassword(model);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
