using Common.Models;

namespace Common.Models
{
    public class RefreshToken: BaseEntity
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime ValidUntil { get; set; }
        public User User { get; set; }
    }
}
