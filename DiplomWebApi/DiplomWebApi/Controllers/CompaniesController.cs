using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private ICompaniesService _companiesService;
        public CompaniesController(ICompaniesService companiesService)
        {
            _companiesService = companiesService;
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetAllCompanies(bool includeDeleted, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _companiesService.GetAllCompanies(includeDeleted, cancellationToken));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPatch("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            await _companiesService.ToggleStatus(id, cancellationToken);
            return NoContent();
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetCompanyInfo(Guid id, CancellationToken cancellationToken) =>
            Ok(await _companiesService.GetCompanyInfo(id, cancellationToken));

        [HttpGet("{id}/packages")]
        [AllowAnonymous]
        //[Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetCompanysPackages(Guid id, CancellationToken cancellationToken) =>
            Ok(await _companiesService.GetCompanysPackages(id, cancellationToken));
    }
}
