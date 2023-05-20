using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using System.Security.Claims;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private IReportsService _reportsService;
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet("weeklyStat/{recorderId}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<ActionResult> GetWeeklyStat(Guid recorderId)
        {
            try
            {
                var reviewerName = this.GetClaim(ClaimTypes.Name);

                var file = await _reportsService.GetWeeklyStat(reviewerName, recorderId);

                Response.Headers.AccessControlExposeHeaders = "Content-Disposition";

                return File(file, System.Net.Mime.MediaTypeNames.Application.Pdf, $"Weekly_Report_Recorder:{recorderId}.pdf");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost("report")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<ActionResult> GetReport(ReportDTO model)
        {
            try
            {
                var reviewerName = this.GetClaim(ClaimTypes.Name);
                var file = await _reportsService.GetReport(reviewerName, model);

                Response.Headers.AccessControlExposeHeaders = "Content-Disposition";

                return File(file, System.Net.Mime.MediaTypeNames.Application.Pdf, $"Report_Recorder:{model.RecorderId}.pdf");
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
