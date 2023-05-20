using DAL.DTOS;
using DiplomWebApi.DTOS;

namespace BL.Services
{
    public interface IRecordingService
    {
        Task<Guid> Authorize(RecorderAuthorizationDTO model);
        Task Create(Guid companyId, RecorderRegistrationDTO model);
        Task<List<RecorderRegistrationReadDTO>> GetAllRecorders(Guid companyId, bool includeDeleted, CancellationToken cancellationToken);
        Task<RecorderDetailsTodayDTO> GetToday(Guid id);
        Task Activate(Guid id, bool activeState);
    }
}
