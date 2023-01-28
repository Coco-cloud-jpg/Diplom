namespace Common.Models
{
    public partial class RecorderRegistration : BaseEntity
    {
        public RecorderRegistration()
        {
            Screenshots = new HashSet<Screenshot>();
        }
        public Guid CompanyId { get; set; }
        public DateTime TimeCreated { get; set; }
        public string HolderName { get; set; }
        public string HolderSurname { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public ICollection<Screenshot> Screenshots { get; set; }
    }

}
