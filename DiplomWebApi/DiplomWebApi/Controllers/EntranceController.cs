using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using DAL.Enums;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/entrance")]
    [ApiController]
    public class EntranceController : ControllerBase
    {
        private IEntranceService _entranceService;
        public EntranceController(IEntranceService entranceService)
        {
            _entranceService = entranceService;
        }

        [HttpGet("chart/{range}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetChart(TimeRange range, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));
            return Ok(await _entranceService.GetChart(companyId, range, cancellationToken));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddEntry(EntranceCreateDTO model)
        {
            await _entranceService.AddEntry(model);
            return Ok();
        }

        [HttpGet("week/{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetAll(Guid id) =>
            Ok(await _entranceService.GetAll(id));
    }
}
