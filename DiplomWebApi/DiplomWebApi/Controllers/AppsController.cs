using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.DTOS;
using DAL.Enums;
using BL.Helpers;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/apps")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        private IAppsService _appsService;
        public AppsController(IAppsService appsService)
        {
            _appsService = appsService;
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> Get()
        //{
        //    return Ok(_unitOfWork.AlertRuleRepository.DbSet.ToList());
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddEntry(AppInfoSTransferDTO model)
        {
            await _appsService.AddEntry(model);
            return Ok();
        }
        [HttpGet("chart/{range}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetChart(TimeRange range, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            return Ok(await _appsService.GetChart(companyId, range, cancellationToken));
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetTodayApps(Guid id) =>
            Ok(await _appsService.GetTodayApps(id));
    }
}
