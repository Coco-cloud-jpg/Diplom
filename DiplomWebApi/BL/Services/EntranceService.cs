using BL.Helpers;
using Common.Models;
using DAL.DTOS;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class EntranceService: IEntranceService
    {
        private List<string> _daysOfWeek = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private IScreenUnitOfWork _unitOfWork;
        public EntranceService(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ChartEntranceDTOShort>> GetChart(Guid companyId, TimeRange range, CancellationToken cancellationToken)
        {
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

            return await _unitOfWork.ChartEntranceDTORepository.DbSet.FromSqlRaw(query).Select(item => new ChartEntranceDTOShort
            {
                Entries = item.Entries,
                Violations = item.Violations,
                Warnings = item.Warnings,
                HolderName = item.HolderName
            }).OrderByDescending(item => item.Entries).ToListAsync(cancellationToken);
        }
        public async Task AddEntry(EntranceCreateDTO model)
        {
            await _unitOfWork.EntryRepository.Create(new Entry
            {
                Id = Guid.NewGuid(),
                RecorderId = model.RecorderId,
                Seconds = model.Seconds,
                Created = DateTime.UtcNow
            }, CancellationToken.None);

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        }
        public async Task<Dictionary<string, double>> GetAll(Guid id)
        {
            int sundayOffset = DateTime.Today.DayOfWeek == 0 ? 7 : 0;
            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - sundayOffset);
            var entries = await _unitOfWork.EntryRepository.DbSet.Where(item => item.RecorderId == id && item.Created > weekStart).ToListAsync();

            var groupedEntries = entries.GroupBy(p => p.Created.Date, g => new GroupEntryTime { Seconds = g.Seconds, Created = g.Created },
                (key, g) => new GroupResult<DateTime> { Key = key, Data = g.ToList() });
            var calculatedEntriesByDate = new Dictionary<string, double>();

            foreach (var item in _daysOfWeek)
            {
                calculatedEntriesByDate.Add(item, 0);
            }

            DateTime temp = DateTime.MinValue;

            foreach (var entry in groupedEntries)
                calculatedEntriesByDate[(entry.Key.DayOfWeek).ToString()] = entry.Data.Sum(item => item.Seconds);

            return calculatedEntriesByDate;
        }
    }
}
