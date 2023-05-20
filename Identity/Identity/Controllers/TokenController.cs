using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL.Services;
using DAL.DTO;

namespace Identity.Controllers
{
    [Route("api/token")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        //[HttpGet]
        //public ActionResult Get(string val)
        //{
        //    return Ok(_cryptoService.ComputeSHA256(val));
        //}
        [HttpPost]
        public async Task<ActionResult> CreateBearerToken(AuthenticationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Bad credentials!");

                return Ok(await _tokenService.CreateBearerToken(request));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("refresh/{token}")]
        public async Task<ActionResult<AuthenticationResponse>> Refresh(string token)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Bad credentials");

                return Ok(await _tokenService.Refresh(token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
