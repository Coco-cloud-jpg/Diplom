namespace Common.Models
{
    public class Company: BaseEntity
    {
        public Company()
        {
            Users = new HashSet<User>();
            RecorderRegistrations = new HashSet<RecorderRegistration>();
            AlertRules = new HashSet<AlertRule>();
        }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<RecorderRegistration> RecorderRegistrations { get; set; }
        public ICollection<AlertRule> AlertRules { get; set; }
        public virtual Country Country { get; set; }
    }
}
