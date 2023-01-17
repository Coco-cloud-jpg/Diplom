using Identity.DTO;
using Common.Models;

namespace Identity.Services.Interfaces
{
    public interface IJwtService
    {
        string CreateAccessToken(User user);
        string CreateRefreshToken();
    }
}
