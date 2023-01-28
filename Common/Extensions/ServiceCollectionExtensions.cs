using Azure.Storage.Blobs;
using Common.Emails;
using Common.Services;
using Common.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBlob(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(x => new BlobServiceClient(connectionString));
            services.AddSingleton<IBlobService, BlobService>();
        }
        
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidIssuer = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
                        )
                    };
                });
        }

        public static void AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        public static void AddKeyVault(this ConfigurationManager configuration)
        {
            configuration.AddAzureKeyVault(Environment.GetEnvironmentVariable("diploma_key_vault"),
                Environment.GetEnvironmentVariable("diploma_common"),
                Environment.GetEnvironmentVariable("diploma_common_secret"));
        }
    }
}
