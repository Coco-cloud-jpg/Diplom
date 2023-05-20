using DAL.DTOS;

namespace BL.Services
{
    public interface IAlertsService
    {   
        Task<List<ShortEntityModelGuid>> GetRecordersInfo(Guid companyId, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
        Task Create(Guid companyId, AlertRuleCreateDTO model, CancellationToken cancellationToken);
        Task Update(Guid id, AlertRuleCreateDTO model, CancellationToken cancellationToken);
        Task<List<AlertRuleReadDTO>> GetAllRules(Guid companyId, int page, int pageSize, CancellationToken cancellationToken);

    }
}
