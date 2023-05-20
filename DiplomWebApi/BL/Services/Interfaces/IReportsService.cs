using DAL.DTOS;

namespace BL.Services
{
    public interface IReportsService
    {
        Task<byte[]> GetWeeklyStat(string reviewerName, Guid recorderId);
        Task<byte[]> GetReport(string reviewerName, ReportDTO model);
    }
}
