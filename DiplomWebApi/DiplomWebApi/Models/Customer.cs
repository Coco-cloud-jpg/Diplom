using Common.Models;

namespace ScreenMonitorService.Models
{
    public partial class Customer: BaseEntity
    {
        public Customer()
        {
            RecorderRegistrations = new HashSet<RecorderRegistration>();
            Screenshots = new HashSet<Screenshot>();
        }
        public string Name { get; set; } = null!;
        public DateTime DateCreated { get; set; }

        public virtual ICollection<RecorderRegistration> RecorderRegistrations { get; set; }
        public virtual ICollection<Screenshot> Screenshots { get; set; }
    }
}
