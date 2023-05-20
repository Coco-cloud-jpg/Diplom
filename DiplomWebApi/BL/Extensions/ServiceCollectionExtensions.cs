using BL.Services;
using BL.Services.Interfaces;
using DAL.Interfaces;
using DAL.Repositories.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBL(this IServiceCollection services)
        {
            services.AddScoped<IScreenUnitOfWork, ScreenUnitOfWork>();
            services.AddSingleton<IOcrService, OcrService>();
            services.AddTransient<IAlertsService, AlertsService>();
            services.AddTransient<IAppsService, AppsService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<ICompaniesService, CompaniesService>();
            services.AddTransient<IEntranceService, EntranceService>();
            services.AddTransient<IPackagesService, PackagesService>();
            services.AddTransient<IPheripheralActivityService, PheripheralActivityService>();
            services.AddTransient<IRecordingService, RecordingService>();
            services.AddTransient<IReportsService, ReportsService>();
            services.AddTransient<IRequestsService, RequestsService>();
            services.AddTransient<IScreenshotService, ScreenshotService>();
            services.AddTransient<IScreenshotTakeService, ScreenshotTakeService>();
            services.AddTransient<ITransactionsService, TransactionsService>();
            services.AddTransient<IWarningsService, WarningsService>();
        }
    }
}
