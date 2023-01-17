using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using ScreenMonitorService.Extensions;
using ScreenMonitorService.Models;

var builder = WebApplication.CreateBuilder(args);
var corsPolicy = "main";
// Add services to the container.

var configs = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors((a) => a.AddPolicy(corsPolicy, policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddDbContext<ScreenContext>(opt => opt.UseSqlServer(configs.GetConnectionString("SQLServer")));
builder.Services.AddServices();
builder.Services.AddBlob(configs.GetConnectionString("Blob"));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
