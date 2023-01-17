using Common.Models;
using Identity.DTO;
using Identity.Interfaces;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        public RegisterController(IIdentityUnitOfWork identityUnitOfWork, ICryptoService cryptoService)
        {
            _identityUnitOfWork = identityUnitOfWork;
            _cryptoService = cryptoService;
        }
        [HttpPost("user")]
        [Authorize(Roles = "System Admin")]
        public async Task<ActionResult> Register(UserCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad credentials");

            await _identityUnitOfWork.UserRepository.Create(new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = _cryptoService.ComputeSHA256(model.Password),
                RoleId = model.RoleId,
                CompanyId = model.CompanyId
            }, CancellationToken.None);

            await _identityUnitOfWork.SaveChangesAsync(CancellationToken.None);

            return Ok();
        }
    }
}
