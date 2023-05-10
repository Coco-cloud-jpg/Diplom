namespace Common.Models
{
    public class PackageTypeCompany: BaseEntity
    {
        public Guid PackageTypeId { get; set; }
        public Guid CompanyId { get; set; }
        public short Count { get; set; }
        public DateTime DateModified { get; set; }

        public virtual Company Company { get; set; }
        public virtual PackageType PackageType { get; set; }
    }
}
