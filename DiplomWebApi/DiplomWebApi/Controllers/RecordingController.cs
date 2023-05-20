using Common.Extensions;
using DiplomWebApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/recordings")]
    [ApiController]
    public class RecordingController : ControllerBase
    {
        private IRecordingService _recordingService;
        public RecordingController(IRecordingService recordingService)
        {
            _recordingService = recordingService;
        }

        [HttpPost("authorize")]
        [AllowAnonymous]
        public async Task<IActionResult> Authorize(RecorderAuthorizationDTO model)
        {
            try
            {
                return Ok(await _recordingService.Authorize(model));
            }
            catch (Exception)
            {
                return Unauthorized();
            }

        }
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Create(RecorderRegistrationDTO model)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                await _recordingService.Create(companyId, model);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetAllRecorders(bool includeDeleted, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                return Ok(await _recordingService.GetAllRecorders(companyId, includeDeleted, cancellationToken));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetToday(Guid id)
        {
            try
            {
                return Ok(await _recordingService.GetToday(id));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpPatch("activate/{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Activate(Guid id, bool activeState)
        {
            try
            {
                await _recordingService.Activate(id, activeState);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
