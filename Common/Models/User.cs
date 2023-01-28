using Common.Models;

namespace Common.Models
{
    public partial class User : BaseEntity
    {
        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Guid RoleId { get; set; }
        public Nullable<Guid> CompanyId { get; set; }
        public Role Role { get; set; }
        public virtual Company Company { get; set; }
        public Guid? PasswordResetId { get; set; }
        public virtual PasswordReset PasswordReset { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
