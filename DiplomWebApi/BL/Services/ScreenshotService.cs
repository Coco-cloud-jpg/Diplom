using BL.Extensions;
using BL.Helpers;
using BL.Services.Interfaces;
using Common.Models;
using Common.Services.Interfaces;
using DAL.DTOS;
using DAL.Enums;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Collections.Concurrent;

namespace BL.Services
{
    public class ScreenshotService: IScreenshotService
    {
        private IScreenUnitOfWork _unitOfWork;
        private IBlobService _blobService;
        public ScreenshotService(IScreenUnitOfWork unitOfWork, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }
        public async Task<List<ChartDTOShort>> GetChart(Guid companyId, TimeRange range, CancellationToken cancellationToken)
        {
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

            return await _unitOfWork.ChartDTORepository.DbSet.FromSqlRaw(query).Select(item => new ChartDTOShort
            {
                DatePart = item.DatePart,
                Data = item.Data
            }).ToListAsync(cancellationToken);
        }
        public async Task UpdateMark(Guid recorderId, Guid screenshotId, AlertState mark)
        {
            var itemToUpdate = await _unitOfWork.ScreenshotRepository.DbSet
                .FirstOrDefaultAsync(item => item.RecorderId == recorderId && item.Id == screenshotId);

            if (itemToUpdate == null)
                throw new ArgumentNullException();

            itemToUpdate.Mark = mark;
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<RecorderInfoData> GetScreenshots(int page, int pageSize, Guid recorderId, bool onlyWarnings, bool onlyViolations, CancellationToken cancellationToken)
        {
            var recorderInfo = await _unitOfWork.RecorderRegistrationRepository.GetById(recorderId, cancellationToken);

            if (!recorderInfo.IsActive)
                throw new ArgumentNullException();

            var screenshots = await _unitOfWork.ScreenshotRepository.DbSet.Where(item => item.RecorderId == recorderId &&
            ((onlyWarnings && onlyViolations ?
                    item.Mark != AlertState.None :
                        onlyWarnings ?
                            item.Mark == AlertState.InternalWarning :
                       onlyViolations ? item.Mark == AlertState.SubmittedViolation : true))
            ).Select(item => new ScreenshotReadDTO
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

            var totalTask = _unitOfWork.ScreenshotRepository.DbSet.Where(item => item.RecorderId == recorderId &&
                        ((onlyWarnings && onlyViolations ?
                    item.Mark != AlertState.None :
                        onlyWarnings ?
                            item.Mark == AlertState.InternalWarning :
                       onlyViolations ? item.Mark == AlertState.SubmittedViolation : true))
            ).CountAsync();

            tasks.Append(totalTask);

            await Task.WhenAll(tasks);

            return new RecorderInfoData
            {
                Data = bag.ToList().OrderByDescending(item => item.TimeCreated),
                Total = await totalTask,
                HolderFullName = $"{recorderInfo.HolderName} {recorderInfo.HolderSurname}"
            };
        }
    }
}
