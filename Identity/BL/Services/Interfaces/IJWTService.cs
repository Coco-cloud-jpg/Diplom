using Common.Models;

namespace BL.Services.Interfaces
{
    public interface IJwtService
    {
        string CreateAccessToken(User user, out long validUntil);
        string CreateRefreshToken();
    }
}
