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
        Task SaveChangesAsync(CancellationToken cancel);
    }
}
