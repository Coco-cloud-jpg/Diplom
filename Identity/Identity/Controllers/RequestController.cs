using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/requests")]
    [ApiController]
    [Authorize]
    public class RequestController : ControllerBase
    {
        //private IIdentityUnitOfWork _unitOfWork;
        //public RequestController(IIdentityUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        //[HttpGet("model")]
        //public async Task<ActionResult> GetPaymentModel()
        //{
        //    return Ok();
        //}
    }
}
