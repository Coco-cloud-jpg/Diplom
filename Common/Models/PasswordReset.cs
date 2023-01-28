namespace Common.Models
{
    public class PasswordReset: BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
