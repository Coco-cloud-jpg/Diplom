using BL.Services;
using BL.Services.Interfaces;
using DAL.Interfaces;
using DAL.Repositories.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBl(this IServiceCollection services)
        {
            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRegisterService, RegisterService>();
            services.AddTransient<IAccountService, AccountService>();
        }
    }
}
