using Common.Models;

namespace DiplomWebApi.DTOS
{
    public class ScreenshotCreateDTO
    {
        public string Base64 { get; set; }
        public Guid RecorderId { get; set; }

    }

    public class ScreenshotReadDTO
    {
        public DateTime TimeCreated { get; set; }
        public Guid Id { get; set; }
        public string Source { get; set; }
        public AlertState Mark { get; set; }
    }
}
