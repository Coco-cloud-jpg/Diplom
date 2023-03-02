using Common.Models;
using Newtonsoft.Json;

namespace RecordingService.DTOS
{
    public class WeeklyReportDTO: BaseEntity
    {
        public string DayOfWeekString { get => DayOfWeek.DayOfWeek.ToString(); }
        public DateTime DayOfWeek { get; set; }
        public long TimeWorked { get; set; }
        public double KeyboardActivity { get; set; }
        public double MouseActivity { get; set; }
        public int Screenshots { get; set; }
        public string AppUsage { get; set; }
        public List<AppFullInfoWithId> AppUsageModels 
        { 
            get 
            {
                if (_appUsageModels == null)
                {
                    _appUsageModels = JsonConvert.DeserializeObject<List<AppFullInfoWithId>>(AppUsage) ?? new List<AppFullInfoWithId>();
                }

                return _appUsageModels;
            }
        }
    
        private List<AppFullInfoWithId> _appUsageModels = null;
    }
}
