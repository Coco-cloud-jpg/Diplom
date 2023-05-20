using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.Enums;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/screenshots")]
    [ApiController]
    public class ScreenshotsController : ControllerBase
    {
        private IScreenshotService _screenshotService;
        public ScreenshotsController(IScreenshotService screenshotService)
        {
            _screenshotService = screenshotService;
        }
        [HttpGet("chart/{range}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetChart(TimeRange range, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));
            return Ok(await _screenshotService.GetChart(companyId, range, cancellationToken));
        }
        [HttpPatch("{recorderId}/{screenshotId}/{mark}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> UpdateMark(Guid recorderId, Guid screenshotId, AlertState mark)
        {
            await _screenshotService.UpdateMark(recorderId, screenshotId, mark);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetScreenshots(int page, int pageSize, Guid recorderId, bool onlyWarnings, bool onlyViolations, CancellationToken cancellationToken) =>
            Ok(await _screenshotService.GetScreenshots(page, pageSize, recorderId, onlyWarnings, onlyViolations, cancellationToken));
    }
}
