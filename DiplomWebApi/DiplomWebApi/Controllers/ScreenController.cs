using Common.Models;
using Common.Services.Interfaces;
using DiplomWebApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenMonitorService.Interfaces;
using ScreenMonitorService.Models;
using SixLabors.ImageSharp;
using System.Collections.Concurrent;

namespace DiplomWebApi.Controllers
{
    [Route("api/screen")]
    [ApiController]
    [AllowAnonymous]
    public class ScreenController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        private IBlobService _blobService;
        public ScreenController(IScreenUnitOfWork unitOfWork, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }

        [HttpPost]
        public async Task<IActionResult> AddScreenShot([FromBody] ScreenshotCreateDTO model)
        {
            try
            {
                var companyId = await _unitOfWork.RecorderRegistrationRepository.DbSet
                    .Where(item => item.Id == model.RecorderId && item.IsActive)
                    .Select(item => item.CompanyId).FirstOrDefaultAsync();

                if (companyId == null)
                    return BadRequest();

                //var dayStart = (DateTime)DateTime.UtcNow.Date;

                //var screenshotsToday = await _unitOfWork.ScreenshotRepository.DbSet.Where(item => item.RecorderId == model.RecorderId && item.DateCreated > dayStart).CountAsync();

                //if (screenshotsToday > RecordingService.Constants.ScreenshotADayMax)
                //    return null;
                
                var img = Image.Load(Convert.FromBase64String(model.Base64));
                var dateTimeNow = DateTime.UtcNow;

                var screenId = Guid.NewGuid();
                var path = $"{companyId}/{model.RecorderId}/{screenId}.jpeg";
                await _blobService.UploadFileBlobAsync("screenshots", model.Base64, path);

                await _unitOfWork.ScreenshotRepository.Create(new Screenshot
                {
                    Id = screenId,
                    RecorderId = model.RecorderId,
                    DateCreated = dateTimeNow,
                    StorePath = path,
                }, CancellationToken.None);

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);

                return Ok();
            }
            catch (Exception e)
            {
                var s = 2;
                throw;
            }
        }
    }
}
