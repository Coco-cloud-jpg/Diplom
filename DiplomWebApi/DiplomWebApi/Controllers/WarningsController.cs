using Common.Extensions;
using Common.Models;
using Common.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;
using RecordingService.Extensions;
using ScreenMonitorService.Interfaces;
using System.Security.Claims;

namespace DiplomWebApi.Controllers
{
    [Route("api/warnings")]
    [ApiController]
    public class WarningsController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        private IBlobService _blobService;
        public WarningsController(IScreenUnitOfWork unitOfWork, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }
        [HttpPost("{screenshotId}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Add(Guid screenshotId, WarningCommentAddDTO model, CancellationToken cancellationToken)
        {
            try
            {
                var userId = Guid.Parse(this.GetClaim(ClaimTypes.NameIdentifier));

                var screenshot = await _unitOfWork.ScreenshotRepository.GetById(screenshotId, cancellationToken);

                if (screenshot == null)
                    return NotFound();

                screenshot.Mark = (AlertState)model.Mark;

                if (model.PostComment)
                {
                    await _unitOfWork.CommentRepository.Create(new Comment
                    {
                        DatePosted = DateTime.UtcNow,
                        CreatorId = userId,
                        ScreenshotId = screenshotId,
                        Text = model.Text
                    }, cancellationToken);
                }

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);

                return Ok();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetAllWarnings(int page, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                var screenshotEntry = await _unitOfWork.ScreenshotRepository.DbSet
                    .Include(item => item.Recorder)
                    .Where(item => item.Recorder.CompanyId == companyId && item.Mark == AlertState.InternalWarning)
                    .OrderByDescending(item => item.DateCreated)
                    .Skip(page)
                    .Select(item => new ScreenshotWarning
                    {
                        Id = item.Id,
                        Path = item.StorePath
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                var totalTask = _unitOfWork.ScreenshotRepository.DbSet
                    .Include(item => item.Recorder)
                    .Where(item => item.Recorder.CompanyId == companyId && item.Mark == AlertState.InternalWarning)
                    .CountAsync(cancellationToken);

                if (screenshotEntry == null)
                    return NotFound();

                screenshotEntry.Base64 = $"data:image/jpeg;base64,{(await _blobService.GetBlobAsync("screenshots", screenshotEntry.Path)).ToBase64String()}";

                return Ok(new GridResult<ScreenshotWarning> { 
                    Data = new List<ScreenshotWarning> { screenshotEntry },
                    Total = await totalTask
                });
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
