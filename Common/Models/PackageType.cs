namespace Common.Models
{
    public class PackageType: BaseEntity
    {
        public PackageType()
        {
            PackageTypeCompanies = new HashSet<PackageTypeCompany>();
            PackageUpgradeRequests = new HashSet<PackageUpgradeRequest>();
        }
        public string Name { get; set; }
        public ushort MaxUsersCount { get; set; }
        public ushort MaxRecordersCount { get; set; }
        public decimal Price { get; set; }
        public short Currency { get; set; }

        public ICollection<PackageTypeCompany> PackageTypeCompanies { get; set; }
        public ICollection<PackageUpgradeRequest> PackageUpgradeRequests { get; set; }
    }

    public enum Currency
    {
        USD,
        EUR,
        UAH
    }
}
