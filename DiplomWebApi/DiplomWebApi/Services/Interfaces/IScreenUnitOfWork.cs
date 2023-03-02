using Common.Interfaces;
using Common.Models;
using RecordingService.DTOS;
using ScreenMonitorService.Models;

namespace ScreenMonitorService.Interfaces
{
    public interface IScreenUnitOfWork
    {
        IGenericRepository<Company> CompanyRepository { get; }
        IGenericRepository<RecorderRegistration> RecorderRegistrationRepository { get; }
        IGenericRepository<Screenshot> ScreenshotRepository { get; }
        IGenericRepository<RecorderRegistrationReadDTO> RecorderRegistrationDTORepository { get; }
        IGenericRepository<AppUsageDTO> AppUsageDTORepository { get; }
        IGenericRepository<Entry> EntryRepository { get; }
        IGenericRepository<PheripheralActivity> PheripheralActivityRepository { get; }
        IGenericRepository<ApplicationInfo> ApplicationInfoRepository { get; }
        IGenericRepository<ApplicationUsageInfo> ApplicationUsageInfoRepository { get; }
        IGenericRepository<WeeklyReportDTO> WeeklyReportDTORepository { get; }
        IGenericRepository<ChartDTO> ChartDTORepository { get; }
        IGenericRepository<ChartEntranceDTO> ChartEntranceDTORepository { get; }
        IGenericRepository<AlertRule> AlertRuleRepository { get; }
        Task SaveChangesAsync(CancellationToken cancel);
    }
}
