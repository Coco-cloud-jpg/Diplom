using RecordingService.Services;
using RecordingService.Services.Interfaces;
using ScreenMonitorService.Interfaces;
using ScreenMonitorService.Repositories.Repository;

namespace ScreenMonitorService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IScreenUnitOfWork, ScreenUnitOfWork>();
            services.AddSingleton<IOcrService, OcrService>();
        }
    }
}
