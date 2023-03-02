using Common.Models;

namespace RecordingService.DTOS
{
    public class ChartDTO: BaseEntity
    {
        public int DatePart { get; set; }
        public double Data { get; set; }
    }

    public class ChartDTOShort
    {
        public int DatePart { get; set; }
        public double Data { get; set; }
    }

    public class ChartEntranceDTO : BaseEntity
    {
        public string HolderName { get; set; }
        public int Entries { get; set; }
        public int Violations { get; set; }
        public int Warnings { get; set; }
    }
    public class ChartEntranceDTOShort
    {
        public string HolderName { get; set; }
        public int Entries { get; set; }
        public int Violations { get; set; }
        public int Warnings { get; set; }
    }
}
