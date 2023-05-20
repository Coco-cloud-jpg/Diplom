using Common.Models;
using DAL.DTO;

namespace BL.Services
{
    public interface ITokenService
    {
        Task<AuthenticationResponse> CreateBearerToken(AuthenticationRequest request);
        Task<AuthenticationResponse> Refresh(string token);
    }
}
