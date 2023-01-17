using Common.Models;

namespace ScreenMonitorService.Models
{
    public class Screenshot: BaseEntity
    {
        public Guid CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        public string StorePath { get; set; }
        public string SenderName { get; set; }

        public Customer Customer { get; set; }
    }
}
