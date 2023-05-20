using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL.Services;
using DAL.DTOS;

namespace DiplomWebApi.Controllers
{
    [Route("api/screen")]
    [ApiController]
    [AllowAnonymous]
    public class ScreenController : ControllerBase
    {
        private IScreenshotTakeService _screenService;
        public ScreenController(IScreenshotTakeService screenService)
        {
            _screenService = screenService;
        }
        //TODO Add recorder token login table, though only one instance can add screenshots via login
        [HttpPost]
        public async Task<IActionResult> AddScreenShot([FromBody] ScreenshotCreateDTO model)
        {
            await _screenService.AddScreenShot(model);
            return Ok();
        }
    }
}
