using BL.Services.Interfaces;
using Common.Models;
using DAL.DTO;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace BL.Services
{
    public class TokenService: ITokenService
    {
        private readonly IJwtService _jwtService;
        private readonly ICryptoService _cryptoService;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        private readonly IConfiguration _con;
        public TokenService(IJwtService jwtService, IIdentityUnitOfWork identityUnitOfWork, ICryptoService cryptoService, IConfiguration con)
        {
            _jwtService = jwtService;
            _identityUnitOfWork = identityUnitOfWork;
            _cryptoService = cryptoService;
            _con = con;
        }
        public async Task<AuthenticationResponse> CreateBearerToken(AuthenticationRequest request)
        {
            var user = await _identityUnitOfWork.UserRepository.DbSet.Include(item => item.Role)
                .Include(item => item.Company).FirstOrDefaultAsync(item => item.Email == request.Email);

            if (user == null || !user.Password.Equals(_cryptoService.ComputeSHA256(request.Password)))
                throw new ArgumentNullException("Bad credentials!");

            if (!user.IsActive)
                throw new ArgumentNullException("Your account is disabled!");

            if (user.RoleId != Guid.Parse(Common.Constants.Role.SystemAdmin) && !user.Company.IsActive)
                throw new ArgumentNullException("Your company is deleted!");

            var lastToken = await _identityUnitOfWork.RefreshTokenRepository.DbSet.FirstOrDefaultAsync(item => item.UserId == user.Id);

            if (lastToken != null)
                await _identityUnitOfWork.RefreshTokenRepository.Delete(lastToken.Id, CancellationToken.None);

            var response = await CreateTokens(user);
            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);
            response.RedirectTo = user.RoleId == Guid.Parse(Common.Constants.Role.SystemAdmin) ? "/home-admin" : "/home";
            return response;
        }
        public async Task<AuthenticationResponse> Refresh(string token)
        {
            var refreshTokenFromDb = await _identityUnitOfWork.RefreshTokenRepository.DbSet
                .Include(item => item.User)
                    .ThenInclude(item => item.Role)
                .FirstOrDefaultAsync(item => item.Token == token);

            if (refreshTokenFromDb == null || refreshTokenFromDb.ValidUntil < DateTime.UtcNow)
                throw new ArgumentNullException();

            await _identityUnitOfWork.RefreshTokenRepository.Delete(refreshTokenFromDb.Id, CancellationToken.None);

            var tokens = await CreateTokens(refreshTokenFromDb.User);
            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);

            return tokens;
        }

        private async Task<AuthenticationResponse> CreateTokens(User user)
        {
            var accessToken = _jwtService.CreateAccessToken(user, out long validUntil);
            var refreshToken = _jwtService.CreateRefreshToken();

            await _identityUnitOfWork.RefreshTokenRepository.Create(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                ValidUntil = DateTime.UtcNow.AddMonths(6),
                UserId = user.Id
            }, CancellationToken.None);

            return new AuthenticationResponse
            {
                Access = accessToken,
                Refresh = refreshToken,
                ValidUntil = validUntil
            };
        }
    }
}
