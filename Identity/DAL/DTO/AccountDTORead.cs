namespace DAL.DTO
{
    public class AccountDTORead
    {
        public List<string> Routes { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class UserInfo
    {
        public string FullName { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string RoleName { get; set; }
    }
}
