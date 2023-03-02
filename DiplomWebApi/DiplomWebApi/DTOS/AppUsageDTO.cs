using Common.Models;

namespace RecordingService.DTOS
{
    public class AppUsageDTO: BaseEntity
    {
        public string Name { get; set; }
        public string IconBase64 { get; set; }
        public uint Seconds { get; set; }
    }
    public class AppInfoBase
    {
        public uint Seconds { get; set; }
        public string Name { get; set; }
    }
    public class AppFullInfo: AppInfoBase
    {
        public string IconBase64 { get; set; }
    }
    public class AppsIconModel: BaseEntity
    {
        public string IconBase64 { get; set; }
    }
    public class AppFullInfoWithId : AppInfoBase
    {
        public Guid Id { get; set; }
    }

    public class AppInfoSTransferDTO
    {
        public IEnumerable<AppFullInfo> AppsInfo { get; set; }
        public Guid RecorderId { get; set; }
    }
}
