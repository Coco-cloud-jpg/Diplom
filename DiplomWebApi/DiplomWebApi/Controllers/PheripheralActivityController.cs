using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/pheripheral")]
    [ApiController]
    public class PheripheralActivityController : ControllerBase
    {
        private IPheripheralActivityService _pheripheralActivityService;
        public PheripheralActivityController(IPheripheralActivityService pheripheralActivityService)
        {
            _pheripheralActivityService = pheripheralActivityService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddEntry(PheripheralActivityDTO model)
        {
            await _pheripheralActivityService.AddEntry(model);
            return Ok();
        }
    }
}
