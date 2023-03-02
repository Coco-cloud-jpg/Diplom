namespace Common.Models
{
    public partial class RecorderRegistration : BaseEntity
    {
        public RecorderRegistration()
        {
            Screenshots = new HashSet<Screenshot>();
            Entries = new HashSet<Entry>();
            PheripheralActivites = new HashSet<PheripheralActivity>();
            ApplicationUsageInfo = new HashSet<ApplicationUsageInfo>();
            AlertRules = new HashSet<AlertRule>();
        }
        public Guid CompanyId { get; set; }
        public DateTime TimeCreated { get; set; }
        public string HolderName { get; set; }
        public string HolderSurname { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public ICollection<ApplicationUsageInfo> ApplicationUsageInfo { get; set; }
        public ICollection<Screenshot> Screenshots { get; set; }
        public ICollection<Entry> Entries { get; set; }
        public ICollection<AlertRule> AlertRules { get; set; }
        public ICollection<PheripheralActivity> PheripheralActivites { get; set; }
    }

}
