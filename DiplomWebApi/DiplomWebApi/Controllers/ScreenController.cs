using Common.Models;
using Common.Services.Interfaces;
using DiplomWebApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.Services.Interfaces;
using ScreenMonitorService.Interfaces;
using ScreenMonitorService.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Collections.Concurrent;

namespace DiplomWebApi.Controllers
{
    [Route("api/screen")]
    [ApiController]
    [AllowAnonymous]
    public class ScreenController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        private IOcrService _ocrService;
        private IBlobService _blobService;
        public ScreenController(IScreenUnitOfWork unitOfWork, IBlobService blobService, IOcrService ocrService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
            _ocrService = ocrService;
        }
        //TODO Add recorder token login table, though only one instance can add screenshots via login
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
                img.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(img.Width / 2, img.Height / 2)
                }));

                var dateTimeNow = DateTime.UtcNow;
                
                var screenId = Guid.NewGuid();
                var path = $"{companyId}/{model.RecorderId}/{screenId}.jpeg";
                await _blobService.UploadFileBlobAsync("screenshots", img.ToBase64String(JpegFormat.Instance), path);

                await _unitOfWork.ScreenshotRepository.Create(new Screenshot
                {
                    Id = screenId,
                    RecorderId = model.RecorderId,
                    DateCreated = dateTimeNow,
                    StorePath = path,
                }, CancellationToken.None);

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);

                //not awaited proccessing
                await _ocrService.Process(model.Base64, companyId, model.RecorderId, screenId, _unitOfWork);

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
