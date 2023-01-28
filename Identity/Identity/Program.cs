using Identity.Extensions;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Common.Extensions;

var builder = WebApplication.CreateBuilder(args);
var corsPolicy = "main";
// Add services to the container.

var configuration = builder.Configuration;
configuration.AddKeyVault();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors((a) => a.AddPolicy(corsPolicy, policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(configuration["ConnectionStrings:DiplomaIdentity"]));
builder.Services.AddServices();
builder.Services.AddSwaggerGen();
builder.Services.AddJwt(configuration);
builder.Services.AddEmail(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
