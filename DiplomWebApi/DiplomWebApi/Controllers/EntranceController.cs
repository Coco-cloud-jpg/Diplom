using Common.Extensions;
using Common.Models;
using Common.Services.Interfaces;
using DiplomWebApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RecordingService;
using RecordingService.DTOS;
using RecordingService.Enums;
using RecordingService.Helpers;
using ScreenMonitorService.Interfaces;
using System.Security.Claims;

namespace DiplomWebApi.Controllers
{
    [Route("api/entrance")]
    [ApiController]
    public class EntranceController : ControllerBase
    {
        private List<string> _daysOfWeek = new List<string>() {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private IScreenUnitOfWork _unitOfWork;
        public EntranceController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("chart/{range}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetChart(TimeRange range, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            DateTime wherePart;

            switch (range)
            {
                case TimeRange.Day:
                    wherePart = DateTime.Today.Date;
                    break;
                case TimeRange.Week:
                    wherePart = ChartHelper.GetWeekStart();
                    break;
                case TimeRange.Month:
                    wherePart = ChartHelper.GetMonthStart();
                    break;
                default:
                    throw new InvalidDataException();
            }

            var query = $@"select top(5) r.Id, r.HolderName + ' ' + r.HolderSurname as HolderName,
					(select count(1) from Entries e where RecorderId = r.Id and e.Created > '{wherePart.ToString("yyyy-MM-dd")}') as Entries,
					(select count(1) from Screenshots s where s.RecorderId = r.Id and s.Mark = 1 and s.DateCreated > '{wherePart.ToString("yyyy-MM-dd")}') as Warnings,
					(select count(1) from Screenshots s where s.RecorderId = r.Id and s.Mark = 2 and s.DateCreated > '{wherePart.ToString("yyyy-MM-dd")}') as Violations
				from RecorderRegistrations r where r.CompanyId = '{companyId}'";

            return Ok(await _unitOfWork.ChartEntranceDTORepository.DbSet.FromSqlRaw(query).Select(item => new ChartEntranceDTOShort
            {
                Entries = item.Entries,
                Violations = item.Violations,
                Warnings = item.Warnings,
                HolderName = item.HolderName
            }).OrderByDescending(item => item.Entries).ToListAsync(cancellationToken));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddEntry(EntranceCreateDTO model)
        {
            try
            {
                await _unitOfWork.EntryRepository.Create(new Entry
                {
                    Id = Guid.NewGuid(),
                    RecorderId = model.RecorderId,
                    Seconds = model.Seconds,
                    Created = DateTime.UtcNow
                }, CancellationToken.None);

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);

                return Ok();
            }
            catch (Exception e)
            {
                var s = 2;
                throw;
            }
        }

        [HttpGet("week/{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            int sundayOffset = DateTime.Today.DayOfWeek == 0 ? 7 : 0;
            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - sundayOffset);
            var entries = await _unitOfWork.EntryRepository.DbSet.Where(item => item.RecorderId == id && item.Created > weekStart).ToListAsync();

            var groupedEntries = entries.GroupBy(p => p.Created.Date, g => new GroupEntryTime{ Seconds = g.Seconds, Created = g.Created }, 
                (key, g) => new GroupResult<DateTime>{Key = key, Data = g.ToList()});
            var calculatedEntriesByDate = new Dictionary<string, double>();

            foreach (var item in _daysOfWeek)
            {
                calculatedEntriesByDate.Add(item, 0);
            }

            DateTime temp = DateTime.MinValue;

            foreach (var entry in groupedEntries)
                calculatedEntriesByDate[(entry.Key.DayOfWeek).ToString()] = entry.Data.Sum(item => item.Seconds);

            return Ok(calculatedEntriesByDate);
        }
    }
}
