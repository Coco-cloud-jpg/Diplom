using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using System.Security.Claims;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/warnings")]
    [ApiController]
    public class WarningsController : ControllerBase
    {
        private IWarningsService _warningsService;
        public WarningsController(IWarningsService warningsService)
        {
            _warningsService = warningsService;
        }
        [HttpPost("{screenshotId}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Add(Guid screenshotId, WarningCommentAddDTO model, CancellationToken cancellationToken)
        {
            try
            {
                var userId = Guid.Parse(this.GetClaim(ClaimTypes.NameIdentifier));
                await _warningsService.Add(userId, screenshotId, model, cancellationToken);

                return Ok();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetAllWarnings(int page, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                return Ok(await _warningsService.GetAllWarnings(companyId, page, cancellationToken));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
