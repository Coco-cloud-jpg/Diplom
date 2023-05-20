using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private IRequestsService _requestsService;
        public RequestsController(IRequestsService requestsService)
        {
            _requestsService = requestsService;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetCompanyRequests(Guid id, CancellationToken cancellationToken) =>
            Ok(await _requestsService.GetCompanyRequests(id, cancellationToken));
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)}")]
        public async Task<IActionResult> Create(Guid id, CancellationToken cancellationToken)
        {
            await _requestsService.Create(id, cancellationToken);
            return Ok();
        }
        [HttpPatch("reject/{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Reject(Guid id, CancellationToken cancellationToken)
        {
            await _requestsService.Reject(id, cancellationToken);
            return Ok();
        }
        [HttpPatch("approve{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
        {
            await _requestsService.Approve(id, cancellationToken);
            return Ok();
        }
    }
}
