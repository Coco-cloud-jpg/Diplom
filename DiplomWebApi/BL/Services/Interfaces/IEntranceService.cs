using DAL.DTOS;
using DAL.Enums;

namespace BL.Services
{
    public interface IEntranceService
    {
        Task<List<ChartEntranceDTOShort>> GetChart(Guid companyId, TimeRange range, CancellationToken cancellationToken);
        Task AddEntry(EntranceCreateDTO model);
        Task<Dictionary<string, double>> GetAll(Guid id);
    }
}
