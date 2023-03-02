namespace Common.Models
{
    public class AlertRule: BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Guid? RecorderId { get; set; }
        public string SendToEmail { get; set; }
        public string SerializedWords { get; set; }
        public DateTime DateCreated { get; set; }
        public Company Company { get; set; }
        public RecorderRegistration Recorder { get; set; }
    }
}
