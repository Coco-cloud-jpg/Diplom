namespace Common.Models
{
    public class ApplicationUsageInfo: BaseEntity
    {
        public uint Seconds { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid RecorderId { get; set; }
        public RecorderRegistration Recorder { get; set; }
        public Guid ApplicationId { get; set; }
        public ApplicationInfo ApplicationInfo { get; set; }
    }
}
