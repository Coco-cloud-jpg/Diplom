using Common.Models;
using Newtonsoft.Json;

namespace DAL.DTOS
{
    public class ReportDTO
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime Start { get => DateTime.Parse(StartDate); }
        public DateTime End { get => DateTime.Parse(EndDate); }
        public Guid RecorderId { get; set; }
        public bool IncludeViolatedScreenshots { get; set; }
    }
}
