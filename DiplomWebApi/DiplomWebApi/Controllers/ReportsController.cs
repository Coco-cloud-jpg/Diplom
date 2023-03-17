using Common.Extensions;
using Common.Models;
using Common.Services.Interfaces;
using DiplomWebApi.DTOS;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;
using RecordingService.Helpers;
using ScreenMonitorService.Interfaces;
using System.Security.Claims;
using System.Text;

namespace DiplomWebApi.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        public ReportsController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("weeklyStat/{recorderId}")]
        //[Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<ActionResult> GetWeeklyStat(Guid recorderId)
        {
            var recorder = await _unitOfWork.RecorderRegistrationRepository.GetById(recorderId, CancellationToken.None);

            if (recorder == null)
                return NotFound();

            var reviewerName = "Pavlo Reviwer"; //this.GetClaim(ClaimTypes.Name);

            int sundayOffset = DateTime.Today.DayOfWeek == 0 ? 7 : 0;
            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - sundayOffset);

            var data = await _unitOfWork.WeeklyReportDTORepository.DbSet.FromSqlRaw(GetWeeklyStatisticsQuery(recorderId, weekStart)).ToListAsync();

            var appsId = data.SelectMany(item => item.AppUsageModels).Select(item => item.Id).ToList();

            var appsIcons = await _unitOfWork.ApplicationInfoRepository.DbSet.Where(item => appsId.Contains(item.Id)).Select(item => new AppsIconModel { IconBase64 = item.IconBase64, Id = item.Id }).ToListAsync();

            var html = System.IO.File.ReadAllText("Templates/WeeklyTemplate.html");

            var today = DateTime.UtcNow.Date;

            var totalAlertsCount = await _unitOfWork.ScreenshotRepository.DbSet
                .Where(item => item.RecorderId == recorderId && item.DateCreated > weekStart && item.Mark != AlertState.None)
                .Select(item => item.Mark)
                .ToListAsync();

            html = html.Replace("{{data}}", CreateTable(data, appsIcons, today))
                       .Replace("{{warningsCount}}", totalAlertsCount.Count(item => item == AlertState.InternalWarning).ToString())
                       .Replace("{{errorsCount}}", totalAlertsCount.Count(item => item == AlertState.SubmittedViolation).ToString())
                       .Replace("{{date}}", today.ToShortDateString())
                       .Replace("{{holder}}", $"{recorder.HolderName} {recorder.HolderSurname}")
                       .Replace("{{reviewer}}", reviewerName);

            Response.Headers.AccessControlExposeHeaders = "Content-Disposition";

            return File(ConvertHtmlToPdf(html), System.Net.Mime.MediaTypeNames.Application.Pdf, $"Weekly_Report_Recorder:{recorderId}.pdf");
        }

        private string CreateTable(List<WeeklyReportDTO> data, List<AppsIconModel> appsIcons, DateTime today)
        {
            StringBuilder html = new StringBuilder();

            foreach (var item in data)
            {
                if (item.DayOfWeek > today)
                    break;

                html.Append("<tr>")
                    .Append($"<td>{item.DayOfWeekString}<br/>{item.DayOfWeek.ToShortDateString()}</td>")
                    .Append($"<td>{SecondsConverter.ToString(item.TimeWorked)}</td>")
                    .Append($"<td>{String.Format("{0:0.00}", item.MouseActivity)} %</td>")
                    .Append($"<td>{String.Format("{0:0.00}", item.KeyboardActivity)} %</td>")
                    .Append($"<td>{item.Screenshots}</td>");

                var appsSb = new StringBuilder();
                appsSb.Append("<td colspan='4'><ul>");

                foreach (var app in item.AppUsageModels)
                {
                    var icon = appsIcons.FirstOrDefault(item => item.Id == app.Id);

                    var image = "";

                    if (icon != null)
                        image = $"<img src='data:image/jpeg;base64,{icon.IconBase64}'/>";

                    var percent = app.Seconds / (double)item.TimeWorked *100;

                    if (item.AppUsageModels.Count == 1)
                    {
                        percent = 100;
                    }

                    var color = percent > 50 ? "red" : percent > 25 ? "yellow" : "green";

                    appsSb.Append($"<li><div class='{color} graph' style='width: {(int)(percent / 100.0 * 220)}px'></div>{String.Format("{0:0.00}", percent)} % {image}<br><span class='app-name'>{app.Name}</span></li>");
                }

                appsSb.Append("</ul></td>");

                html.Append(appsSb.ToString());
                html.Append("</tr>");
            }

            return html.ToString();
        }
        private byte[] ConvertHtmlToPdf(string html)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter pdfWriter = new PdfWriter(memoryStream);
                PdfDocument pdfDoc = new PdfDocument(pdfWriter);

                string fontFile = "Templates/Montserrat-Regular.ttf";
                var fontProvider = new DefaultFontProvider(false, false, false);
                FontProgram fontProgram = FontProgramFactory.CreateFont(fontFile);
                fontProvider.AddFont(fontProgram);

                ConverterProperties properties = new ConverterProperties();
                properties.SetFontProvider(fontProvider);

                Document htmlDocument = HtmlConverter.ConvertToDocument(html, pdfDoc, properties);

                htmlDocument.SetMargins(0, 0, 0, 0);
                htmlDocument.Close();
                return memoryStream.ToArray();
            }
        }
        private string GetWeeklyStatisticsQuery(Guid recorderId, DateTime weekStart) =>
            $@"select days.DayOfWeek, 
                      newid() as Id,
            isnull((select sum(e.Seconds) from Entries e
            where e.RecorderId = '{recorderId}' and 
            e.Created > days.DayOfWeek and e.Created < DATEADD(DAY, 1, days.DayOfWeek) group by cast(Created as date)), 0) as TimeWorked,
            isnull((select avg(p.KeyboardActivePercentage) * 100 from PheripheralActivities p
            where p.RecorderId = '{recorderId}' and 
            p.DateCreated > days.DayOfWeek and p.DateCreated < DATEADD(DAY, 1, days.DayOfWeek) group by cast(DateCreated as date)), 0.0) as KeyboardActivity,
            isnull((select avg(p.MouseActivePercentage) * 100 from PheripheralActivities p
            where p.RecorderId = '{recorderId}' and 
            p.DateCreated > days.DayOfWeek and p.DateCreated < DATEADD(DAY, 1, days.DayOfWeek) group by cast(DateCreated as date)), 0.0) as MouseActivity,
            (select count(1) from Screenshots s
            where s.RecorderId = '{recorderId}' and 
            s.DateCreated > days.DayOfWeek and s.DateCreated < DATEADD(DAY, 1, days.DayOfWeek)) as Screenshots,
            isnull((select applicationId as Id, ai.Name, t.seconds from 
                                     (select 
                                     applicationId, sum(seconds) as seconds 
                                     from ApplicationUsageInfos au
                                     where au.RecorderId = '{recorderId}' and au.TimeStamp > days.DayOfWeek and au.TimeStamp < DATEADD(DAY, 1, days.DayOfWeek)
                                     group by applicationId) t
                                     inner join ApplicationInfos ai on ai.Id = t.ApplicationId
                                     order by t.seconds desc for json path), '') as AppUsage
            from 
            (SELECT 
                DATEADD(DAY, number, StartOfWeek) AS DayOfWeek
            FROM 
                master..spt_values
                CROSS APPLY (SELECT '{weekStart.ToString("yyyy-MM-dd")}')
	            AS StartingDates(StartOfWeek)
            WHERE 
                type = 'P' 
                AND number BETWEEN 0 AND 6) as days
            ";
    }
}
