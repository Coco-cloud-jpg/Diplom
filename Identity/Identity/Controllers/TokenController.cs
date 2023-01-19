using Identity.DTO;
using Identity.Interfaces;
using Common.Models;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly ICryptoService _cryptoService;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        private readonly IConfiguration _con;
        public TokenController(IJwtService jwtService, IIdentityUnitOfWork identityUnitOfWork, ICryptoService cryptoService, IConfiguration con)
        {
            _jwtService = jwtService;
            _identityUnitOfWork = identityUnitOfWork;
            _cryptoService = cryptoService;
            _con = con;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_con.AsEnumerable());
        }
        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad credentials");

            var user = await _identityUnitOfWork.UserRepository.DbSet.Include(item => item.Role).FirstOrDefaultAsync(item => item.Email == request.Email);

            if (user == null || !user.Password.Equals(_cryptoService.ComputeSHA256(request.Password)))
                return BadRequest("Bad credentials");

            var lastToken = await _identityUnitOfWork.RefreshTokenRepository.DbSet.FirstOrDefaultAsync(item => item.UserId == user.Id);

            if (lastToken != null)
                await _identityUnitOfWork.RefreshTokenRepository.Delete(lastToken.Id, CancellationToken.None);

            var tokens = await CreateTokens(user);
            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);
            return Ok(tokens);
        }
        [HttpPost("refresh/{token}")]
        public async Task<ActionResult<AuthenticationResponse>> Refresh(string token)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad credentials");

            var refreshTokenFromDb = await _identityUnitOfWork.RefreshTokenRepository.DbSet
                .Include(item => item.User)
                    .ThenInclude(item => item.Role)
                .FirstOrDefaultAsync(item => item.Token == token);

            if (refreshTokenFromDb == null || refreshTokenFromDb.ValidUntil < DateTime.UtcNow)
                return BadRequest();

            await _identityUnitOfWork.RefreshTokenRepository.Delete(refreshTokenFromDb.Id, CancellationToken.None);

            var tokens = await CreateTokens(refreshTokenFromDb.User);
            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);

            return Ok(tokens);
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

            return new AuthenticationResponse { 
                Access = accessToken,
                Refresh = refreshToken,
                ValidUntil = validUntil
            };
        }
    }
}
