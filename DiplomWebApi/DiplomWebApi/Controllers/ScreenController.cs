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
    [Route("api/[controller]")]
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

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize(ScreenAuthorizationDTO model)
        {
            return Ok(true);
        }
        //remove
        [HttpPost("/all")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.CustomerRepository.GetAll(CancellationToken.None));
        }
        //remove
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _blobService.ListBlobAsync("screenshots"));
        }
        //remove
        [HttpGet("one")]
        public async Task<FileStreamResult> GetAlloo()
        {
            return new FileStreamResult(await _blobService.GetBlobAsync("screenshots", "11d5234b-006d-46e1-bf16-fbcb0776417a/0A-00-27-00-00-2A/cd8efd46-08d6-4b86-ad53-6788b9bb9bb2.jpeg"), "image/jpeg");
        }
        [HttpPost]
        public async Task<IActionResult> AddScreenShoot([FromBody] ScreenshotCreateDTO model)
        {
            try
            {
                var img = Image.Load(Convert.FromBase64String(model.Base64));
                var dateTimeNow = DateTime.UtcNow;

                Guid? customerId = Guid.Parse("11D5234B-006D-46E1-BF16-FBCB0776417A");
                //var customerId = await _unitOfWork.RecorderRegistrationRepository.DbSet
                //                    .Where(item => item.MacAddress == model.SenderName)
                //                    .Select(item => item.CustomerId).FirstOrDefaultAsync();

                //if (customerId == null)
                //    return BadRequest();

                var screenId = Guid.NewGuid();
                var path = $"{customerId}/{model.SenderName}/{screenId}.jpeg";
                await _blobService.UploadFileBlobAsync("screenshots", model.Base64, path);

                //Directory.CreateDirectory($"./Storage/{model.SenderName}/{dateTimeNow.Year}/{dateTimeNow.Month}/{dateTimeNow.Day}");

                //img.Save(path);

                await _unitOfWork.ScreenshotRepository.Create(new Screenshot
                {
                    Id = screenId,
                    CustomerId = customerId.Value,
                    DateCreated = dateTimeNow,
                    StorePath = path,
                    SenderName = model.SenderName
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
