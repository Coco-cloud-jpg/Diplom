using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ScreenMonitorService.Extensions;
using ScreenMonitorService.Models;

var builder = WebApplication.CreateBuilder(args);
var corsPolicy = "main";
// Add services to the container.

var configuration = builder.Configuration;

configuration.AddKeyVault();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors((a) => a.AddPolicy(corsPolicy, policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddDbContext<ScreenContext>(opt => opt.UseSqlServer(configuration["ConnectionStrings:DiplomaIdentity"]));
builder.Services.AddServices();
builder.Services.AddJwt(configuration);
builder.Services.AddBlob(configuration["ConnectionStrings:Blob"]);
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
