using Common.Models;

namespace DAL.DTOS
{
    public class EntranceCreateDTO
    {
        public Guid RecorderId { get; set; }
        public uint Seconds { get; set; }
    }
    public class GroupResult<T>
    {
        public T Key { get; set; }
        public IEnumerable<GroupEntryBase> Data { get; set; }
    }

    public class GroupEntryBase
    {
        public uint Seconds { get; set; }
    }
    public class GroupEntryTime: GroupEntryBase
    {
        public DateTime Created { get; set; }
    }

    public class GroupEntryApps : GroupEntryBase
    {
        public string IconBase64 { get; set; }
        public string Name { get; set; }
    }

    public class AppKey
    {
        public string IconBase64 { get; set; }
        public string Name { get; set; }
    }
}
