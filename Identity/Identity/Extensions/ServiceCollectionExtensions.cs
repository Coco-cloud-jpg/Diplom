using Identity.Interfaces;
using Identity.Repositories.Repository;
using Identity.Services;
using Identity.Services.Interfaces;

namespace Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
        }
    }
}
