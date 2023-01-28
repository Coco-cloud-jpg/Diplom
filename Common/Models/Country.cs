namespace Common.Models
{
    public class Country: BaseEntity
    {
        public Country()
        {
            Companies = new HashSet<Company>();
        }
        public string Name { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}
