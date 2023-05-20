namespace DAL.DTOS
{
    public class RequestReadDTO
    {
        public Guid Id { get; set; }
        public string PackageType { get; set; }
        public int PackagesCount { get; set; }
        public DateTime TimePosted { get; set; }

    }
}
