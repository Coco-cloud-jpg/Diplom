using Common.Interfaces;
using Common.Models;
using DAL.DTOS;
using ScreenMonitorService.Models;

namespace DAL.Interfaces
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
        IGenericRepository<CompanyUsersAndRecordersCountDTO> CompanyUsersAndRecordersCountDTORepository { get; }
        IGenericRepository<AlertRule> AlertRuleRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        IGenericRepository<PackageType> PackageTypeRepository { get; }
        IGenericRepository<PackageTypeCompany> PackageTypeCompanyRepository { get; }
        IGenericRepository<BillingTransaction> BillingTransactionRepository { get; }
        IGenericRepository<PackageUpgradeRequest> PackageUpgradeRequestRepository { get; }
        Task SaveChangesAsync(CancellationToken cancel);
    }
}
