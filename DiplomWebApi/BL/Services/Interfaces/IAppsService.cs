using DAL.DTOS;
using DAL.Enums;

namespace BL.Services
{
    public interface IAppsService
    {
        Task AddEntry(AppInfoSTransferDTO model);
        Task<List<AppUsageDTO>> GetChart(Guid companyId, TimeRange range, CancellationToken cancellationToken);
        Task<List<AppUsageDTO>> GetTodayApps(Guid id);
    }
}
