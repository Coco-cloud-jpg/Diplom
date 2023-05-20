using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/alerts")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private IAlertsService _alertsService;
        public AlertsController(IAlertsService alertsService)
        {
            _alertsService = alertsService;
        }
        [HttpGet("recorders")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetRecordersInfo(CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            return Ok(await _alertsService.GetRecordersInfo(companyId, cancellationToken));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _alertsService.Delete(id, cancellationToken);

            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Create(AlertRuleCreateDTO model, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            await _alertsService.Create(companyId, model, cancellationToken);

            return Ok();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Update(Guid id, AlertRuleCreateDTO model, CancellationToken cancellationToken)
        {
            await _alertsService.Update(id, model, cancellationToken);

            return Ok();
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetAllRules(int page, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                return Ok(await _alertsService.GetAllRules(companyId, page, pageSize, cancellationToken));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
