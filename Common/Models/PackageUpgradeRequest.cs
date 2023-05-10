namespace Common.Models
{
    public class PackageUpgradeRequest: BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Guid PackageTypeId { get; set; }
        public short PackagesCount { get; set; }
        public DateTime TimePosted { get; set; }
        public short Status { get; set; }
        public virtual Company Company { get; set; }
        public virtual PackageType PackageType { get; set; }
    }

    public enum RequestStatus
    {
        Pending,
        Rejected,
        Approved
    }
}
