using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/packages")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private IPackagesService _packagesService;
        public PackagesController(IPackagesService packagesService)
        {
            _packagesService = packagesService;
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetAllPackages(CancellationToken cancellationToken) =>
            Ok(await _packagesService.GetAllPackages(cancellationToken));
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Create(PackageTypeDTO model, CancellationToken cancellationToken) =>
            Ok(await _packagesService.Create(model, cancellationToken));
        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Update(Guid id, PackageTypeDTO model, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _packagesService.Update(id, model, cancellationToken));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _packagesService.Delete(id, cancellationToken);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}/companies")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public async Task<IActionResult> GetCompanies(Guid id, CancellationToken cancellationToken) =>
            Ok(await _packagesService.GetCompanies(id, cancellationToken));

        [HttpGet("currency")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.SystemAdmin)}")]
        public IActionResult GetCurrencies() =>
            Ok(_packagesService.GetCurrencies());
    }
}
