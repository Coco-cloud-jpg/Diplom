using Common.Extensions;
using Common.Models;
using Common.Services.Interfaces;
using DiplomWebApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;
using RecordingService.Enums;
using RecordingService.Extensions;
using RecordingService.Helpers;
using ScreenMonitorService.Interfaces;
using System.Collections.Concurrent;

namespace DiplomWebApi.Controllers
{
    [Route("api/screenshots")]
    [ApiController]
    public class ScreenshotsController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        private IBlobService _blobService;
        public ScreenshotsController(IScreenUnitOfWork unitOfWork, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }
        [HttpGet("chart/{range}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetChart(TimeRange range, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            var fromPart = "";

            switch (range)
            {
                case TimeRange.Day:
                    fromPart = ChartHelper.GetThisDayHours();
                    break;
                case TimeRange.Week:
                    fromPart = ChartHelper.GetThisWeekDays();
                    break;
                case TimeRange.Month:
                    fromPart = ChartHelper.GetThisMonthDays();
                    break;
                default:
                    throw new InvalidDataException();
            }

            var addPart = range == TimeRange.Day ? "HOUR" : "DAY";

            var query = $@"select '{companyId}' AS ID, DATEPART({addPart}, dateTable.DatePart) as DatePart,
                CAST((select count(1) from Screenshots s left join RecorderRegistrations r on r.Id = s.RecorderId where 
                    DateCreated > dateTable.DatePart and s.DateCreated < DATEADD({addPart}, 1, dateTable.DatePart)
                        AND r.CompanyId = '{companyId}'
                    ) AS float) as data {fromPart}";

            return Ok(await _unitOfWork.ChartDTORepository.DbSet.FromSqlRaw(query).Select(item => new ChartDTOShort
            {
                DatePart = item.DatePart,
                Data = item.Data
            }).ToListAsync(cancellationToken));
        }
        [HttpPatch("{recorderId}/{screenshotId}/{mark}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> UpdateMark(Guid recorderId, Guid screenshotId, AlertState mark)
        {
            var itemToUpdate = await _unitOfWork.ScreenshotRepository.DbSet
                .FirstOrDefaultAsync(item => item.RecorderId == recorderId && item.Id == screenshotId);

            if (itemToUpdate == null)
                return NotFound();

            itemToUpdate.Mark = mark;
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetScreenshots(int page, int pageSize, Guid recorderId, CancellationToken cancellationToken)
        {
            var recorderInfo = await _unitOfWork.RecorderRegistrationRepository.GetById(recorderId, cancellationToken);

            if (!recorderInfo.IsActive)
                return NotFound();

            var screenshots = await _unitOfWork.ScreenshotRepository.DbSet.Where(item => item.RecorderId == recorderId).Select(item => new ScreenshotReadDTO
            {
                Id = item.Id,
                TimeCreated = item.DateCreated,
                Source = item.StorePath,
                Mark = item.Mark
            }).OrderByDescending(item => item.TimeCreated).Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            var bag = new ConcurrentBag<ScreenshotReadDTO>();
            var tasks = screenshots.Select(async item =>
            {
                bag.Add(new ScreenshotReadDTO
                {
                    Source = $"data:image/jpeg;base64,{(await _blobService.GetBlobAsync("screenshots", item.Source)).ToBase64String()}",
                    TimeCreated = item.TimeCreated,
                    Id = item.Id,
                    Mark = item.Mark
                });
            }).ToArray();

            var totalTask = _unitOfWork.ScreenshotRepository.DbSet.Where(item => item.RecorderId == recorderId).CountAsync();
            tasks.Append(totalTask);

            await Task.WhenAll(tasks);

            return Ok(new RecorderInfoData { 
                Data = bag.ToList().OrderByDescending(item => item.TimeCreated), 
                Total = await totalTask,
                HolderFullName = $"{recorderInfo.HolderName} {recorderInfo.HolderSurname}"
            });

        }
    }
}
