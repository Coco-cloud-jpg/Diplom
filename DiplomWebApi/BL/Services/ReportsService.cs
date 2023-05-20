using Common.Models;
using Common.Services.Interfaces;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.EntityFrameworkCore;
using DAL.DTOS;
using BL.Helpers;
using DAL.Interfaces;
using System.Collections.Concurrent;
using System.Text;
using BL.Extensions;

namespace BL.Services
{
    //TODO: REFACTOR
    public class ReportsService: IReportsService
    {
        private IScreenUnitOfWork _unitOfWork;
        private IBlobService _blobService;
        public ReportsService(IScreenUnitOfWork unitOfWork, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }
        public async Task<byte[]> GetWeeklyStat(string reviewerName, Guid recorderId)
        {
            var recorder = await _unitOfWork.RecorderRegistrationRepository.GetById(recorderId, CancellationToken.None);

            if (recorder == null)
                throw new ArgumentNullException();

            int sundayOffset = DateTime.Today.DayOfWeek == 0 ? 7 : 0;
            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - sundayOffset);

            var query = GetWeeklyStatisticsQuery(recorderId, weekStart);
            var data = await _unitOfWork.WeeklyReportDTORepository.DbSet.FromSqlRaw(query).ToListAsync();

            var appsId = data.SelectMany(item => item.AppUsageModels).Select(item => item.Id).ToList();

            var appsIcons = await _unitOfWork.ApplicationInfoRepository.DbSet.Where(item => appsId.Contains(item.Id)).Select(item => new AppsIconModel { IconBase64 = item.IconBase64, Id = item.Id }).ToListAsync();

            var html = System.IO.File.ReadAllText("../BL/Templates/WeeklyTemplate.html");

            var today = DateTime.UtcNow.Date;

            var totalAlertsCount = await _unitOfWork.ScreenshotRepository.DbSet
                .Where(item => item.RecorderId == recorderId && item.DateCreated > weekStart && item.Mark != AlertState.None)
                .Select(item => item.Mark)
                .ToListAsync();

            html = html.Replace("{{title}}", "Weekly Activity Report")
                       .Replace("{{screenshots}}", string.Empty)
                       .Replace("{{data}}", CreateTable(data, appsIcons, today))
                       .Replace("{{warningsCount}}", totalAlertsCount.Count(item => item == AlertState.InternalWarning).ToString())
                       .Replace("{{errorsCount}}", totalAlertsCount.Count(item => item == AlertState.SubmittedViolation).ToString())
                       .Replace("{{date}}", today.ToShortDateString())
                       .Replace("{{holder}}", $"{recorder.HolderName} {recorder.HolderSurname}")
                       .Replace("{{reviewer}}", reviewerName);

            return ConvertHtmlToPdf(html);
            //Response.Headers.AccessControlExposeHeaders = "Content-Disposition";

            //return File(ConvertHtmlToPdf(html), System.Net.Mime.MediaTypeNames.Application.Pdf, $"Weekly_Report_Recorder:{recorderId}.pdf");
        }
        public async Task<byte[]> GetReport(string reviewerName, ReportDTO model)
        {
            var recorder = await _unitOfWork.RecorderRegistrationRepository.GetById(model.RecorderId, CancellationToken.None);

            if (recorder == null)
                throw new ArgumentNullException();

            var query = GetStatisticsQuery(model.RecorderId, model.Start.ToString("yyyy-MM-dd"), model.End.ToString("yyyy-MM-dd"));
            var data = await _unitOfWork.WeeklyReportDTORepository.DbSet.FromSqlRaw(query).ToListAsync();

            var appsId = data.SelectMany(item => item.AppUsageModels).Select(item => item.Id).ToList();

            var appsIcons = await _unitOfWork.ApplicationInfoRepository.DbSet.Where(item => appsId.Contains(item.Id)).Select(item => new AppsIconModel { IconBase64 = item.IconBase64, Id = item.Id }).ToListAsync();

            var html = System.IO.File.ReadAllText("../BL/Templates/WeeklyTemplate.html");

            var today = DateTime.UtcNow.Date;

            var totalAlertsCount = await _unitOfWork.ScreenshotRepository.DbSet
                .Where(item => item.RecorderId == model.RecorderId &&
                               item.DateCreated > model.Start &&
                               item.DateCreated < model.End &&
                               item.Mark != AlertState.None)
                .Select(item => item.Mark)
                .ToListAsync();

            var screenshotsHtml = new StringBuilder();

            if (model.IncludeViolatedScreenshots)
            {
                var screenshotsPaths = await _unitOfWork.ScreenshotRepository.DbSet
                    .Where(item => item.RecorderId == model.RecorderId &&
                                   item.DateCreated > model.Start &&
                                   item.DateCreated < model.End &&
                                   item.Mark == AlertState.SubmittedViolation)
                    .Select(item => item.StorePath)
                    .ToListAsync();

                var bag = new ConcurrentBag<string>();
                var tasks = screenshotsPaths.Select(async item =>
                {
                    bag.Add($"data:image/jpeg;base64,{(await _blobService.GetBlobAsync("screenshots", item)).ToBase64String()}");
                }).ToArray();

                await Task.WhenAll(tasks);

                screenshotsHtml.Append($"<tr><td><h3>Violated Screenshots</h3></td></tr>");

                foreach (var item in bag)
                {
                    screenshotsHtml.Append($"<tr><td class='screenshot-wrapper'><img src='{item}'</td></tr>");
                }

            }

            html = html.Replace("{{title}}", "Custom Activity Report")
                       .Replace("{{screenshots}}", screenshotsHtml.ToString())
                       .Replace("{{data}}", CreateTable(data, appsIcons, today))
                       .Replace("{{warningsCount}}", totalAlertsCount.Count(item => item == AlertState.InternalWarning).ToString())
                       .Replace("{{errorsCount}}", totalAlertsCount.Count(item => item == AlertState.SubmittedViolation).ToString())
                       .Replace("{{date}}", today.ToShortDateString())
                       .Replace("{{holder}}", $"{recorder.HolderName} {recorder.HolderSurname}")
                       .Replace("{{reviewer}}", reviewerName);
            return ConvertHtmlToPdf(html);
            //Response.Headers.AccessControlExposeHeaders = "Content-Disposition";

            //return File(ConvertHtmlToPdf(html), System.Net.Mime.MediaTypeNames.Application.Pdf, $"Report_Recorder:{model.RecorderId}.pdf");
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

                    var percent = app.Seconds / (double)item.TimeWorked * 100;

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

                string fontFile = "../BL/Templates/Montserrat-Regular.ttf";
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

        private string GetStatisticsQuery(Guid recorderId, string start, string end) =>
            $@"with dateRange as
                (
                  select dt = dateadd(dd, 1, '{start}')
                  where dateadd(dd, 1, '{start}') <= '{end}'
                  union all
                  select dateadd(dd, 1, dt)
                  from dateRange
                  where dateadd(dd, 1, dt) <= '{end}'
                )

                select days.dt as DayOfWeek, 
                                      newid() as Id,
                            isnull((select sum(e.Seconds) from Entries e
                            where e.RecorderId = '{recorderId}' and 
                            e.Created > days.dt and e.Created < DATEADD(DAY, 1, days.dt) group by cast(Created as date)), 0) as TimeWorked,
                            isnull((select avg(p.KeyboardActivePercentage) * 100 from PheripheralActivities p
                            where p.RecorderId = '{recorderId}' and 
                            p.DateCreated > days.dt and p.DateCreated < DATEADD(DAY, 1, days.dt) group by cast(DateCreated as date)), 0.0) as KeyboardActivity,
                            isnull((select avg(p.MouseActivePercentage) * 100 from PheripheralActivities p
                            where p.RecorderId = '{recorderId}' and 
                            p.DateCreated > days.dt and p.DateCreated < DATEADD(DAY, 1, days.dt) group by cast(DateCreated as date)), 0.0) as MouseActivity,
                            (select count(1) from Screenshots s
                            where s.RecorderId = '{recorderId}' and 
                            s.DateCreated > days.dt and s.DateCreated < DATEADD(DAY, 1, days.dt)) as Screenshots,
                            isnull((select applicationId as Id, ai.Name, t.seconds from 
                                                     (select 
                                                     applicationId, sum(seconds) as seconds 
                                                     from ApplicationUsageInfos au
                                                     where au.RecorderId = '{recorderId}' and au.TimeStamp > days.dt and au.TimeStamp < DATEADD(DAY, 1, days.dt)
                                                     group by applicationId) t
                                                     inner join ApplicationInfos ai on ai.Id = t.ApplicationId
                                                     order by t.seconds desc for json path), '') as AppUsage
                            from 
                            dateRange as days
            option (maxrecursion 0)
            ";
    }
}
