using Identity.DTO;
using Identity.Interfaces;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.Constants;
using Common.Extensions;
using System.Security.Claims;

namespace Identity.Controllers
{
    [Route("api/requests")]
    [ApiController]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private IIdentityUnitOfWork _unitOfWork;
        public RequestController(IIdentityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("model")]
        public async Task<ActionResult> GetPaymentModel()
        {
            return Ok();
        }
    }
}
