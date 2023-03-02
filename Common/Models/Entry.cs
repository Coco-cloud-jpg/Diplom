namespace Common.Models
{
    public class Entry: BaseEntity
    {
        public DateTime Created { get; set; }
        public Guid RecorderId { get; set; }
        public uint Seconds { get; set; }
        public RecorderRegistration Recorder { get; set; }
    }
}
