using Common.Interfaces;
using ScreenMonitorService.Models;

namespace ScreenMonitorService.Interfaces
{
    public interface IScreenUnitOfWork
    {
        IGenericRepository<Customer> CustomerRepository { get; }
        IGenericRepository<RecorderRegistration> RecorderRegistrationRepository { get; }
        IGenericRepository<Screenshot> ScreenshotRepository { get; }
        Task SaveChangesAsync(CancellationToken cancel);
    }
}
