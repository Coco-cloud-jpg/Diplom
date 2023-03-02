using Common.Extensions;
using Common.Models;
using Common.Services.Interfaces;
using DiplomWebApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;
using RecordingService.Enums;
using RecordingService.Helpers;
using ScreenMonitorService.Interfaces;

namespace DiplomWebApi.Controllers
{
    [Route("api/apps")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        public AppsController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            return Ok(_unitOfWork.AlertRuleRepository.DbSet.ToList());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddEntry(AppInfoSTransferDTO model)
        {
            try
            {
                var now = DateTime.UtcNow;
                var allApps = await AddNewApps(model.AppsInfo);
                _unitOfWork.ApplicationUsageInfoRepository.DbSet.
                    AddRange(allApps.Select(item => new ApplicationUsageInfo
                    {
                        Seconds = item.Seconds,
                        TimeStamp = now,
                        RecorderId = model.RecorderId,
                        ApplicationId = item.Id
                    }));

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);

                return Ok();
            }
            catch (Exception A)
            {
                var S = 2;
                throw;
            }
        }
        [HttpGet("chart/{range}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetChart(TimeRange range, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            DateTime wherePart;

            switch (range)
            {
                case TimeRange.Day:
                    wherePart = DateTime.Today.Date;
                    break;
                case TimeRange.Week:
                    wherePart = ChartHelper.GetWeekStart();
                    break;
                case TimeRange.Month:
                    wherePart = ChartHelper.GetMonthStart();
                    break;
                default:
                    throw new InvalidDataException();
            }

            var where = @$"inner join RecorderRegistrations as r on au.RecorderId = r.Id where r.CompanyId = '{companyId}' and 
                    au.TimeStamp > '{wherePart.ToString("yyyy-MM-dd")}' ";

            return Ok(await _unitOfWork.AppUsageDTORepository.DbSet.FromSqlRaw(AppQuery(where)).ToListAsync(cancellationToken));
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetTodayApps(Guid id)
        {
            var where = $"where au.RecorderId = '{id}' and au.TimeStamp > '{DateTime.UtcNow.Date.ToString("yyyy-MM-dd")}' ";

            return Ok(await _unitOfWork.AppUsageDTORepository.DbSet.FromSqlRaw(AppQuery(where)).ToListAsync());
        }
        private string AppQuery(string where) =>
             $@"select applicationId as Id, ai.Name, ai.IconBase64, t.seconds from 
                         (select 
                         applicationId, sum(seconds) as seconds 
                         from ApplicationUsageInfos au
                         {where}
                         group by applicationId) t
                         inner join ApplicationInfos ai on ai.Id = t.ApplicationId
                         order by t.seconds desc";
        private async Task<IEnumerable<AppFullInfoWithId>> AddNewApps(IEnumerable<AppFullInfo> appsInfo)
        {
            var dbAppsAll = await _unitOfWork.ApplicationInfoRepository.DbSet.AsNoTracking()
                .Select(item => new ApplicationInfo { Name = item.Name, Id = item.Id }).ToListAsync();

            var appsToAdd = appsInfo
                .Select(item => new ApplicationInfo
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    IconBase64 = item.IconBase64
                }).ToList();

            var namesFromDb = dbAppsAll.Select(item => item.Name);

            var except = appsToAdd.Where(item => !namesFromDb.Contains(item.Name));

            _unitOfWork.ApplicationInfoRepository.DbSet.AddRange(except);

            dbAppsAll.AddRange(except);

            return appsInfo.Select(item => new AppFullInfoWithId
            {
                Name = item.Name,
                Seconds = item.Seconds,
                Id = dbAppsAll.Where(a => a.Name == item.Name).Select(a => a.Id).FirstOrDefault()
            });
        }
    }
}
