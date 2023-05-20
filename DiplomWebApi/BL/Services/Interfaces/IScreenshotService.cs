using BL.Helpers;
using BL.Services.Interfaces;
using Common.Models;
using Common.Services.Interfaces;
using DAL.DTOS;
using DAL.Enums;
namespace BL.Services
{
    public interface IScreenshotService
    {
        Task<List<ChartDTOShort>> GetChart(Guid companyId, TimeRange range, CancellationToken cancellationToken);
        Task UpdateMark(Guid recorderId, Guid screenshotId, AlertState mark);
        Task<RecorderInfoData> GetScreenshots(int page, int pageSize, Guid recorderId, bool onlyWarnings, bool onlyViolations, CancellationToken cancellationToken);
    }
}
