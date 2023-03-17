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
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        public CommentsController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("{screenshotId}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetScreenshots(Guid screenshotId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _unitOfWork.CommentRepository.DbSet
                    .Include(item => item.User)
                    .Where(item => item.ScreenshotId == screenshotId)
                    .OrderByDescending(item => item.DatePosted)
                    .Select(item => new CommentDTO
                    {
                        Text = item.Text,
                        DatePosted = item.DatePosted.ToString("g"),
                        PosterName = $"{item.User.FirstName} {item.User.LastName}"
                    })
                    .ToListAsync(cancellationToken));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpPost("{screenshotId}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Add(Guid screenshotId, CommentAddDTO model, CancellationToken cancellationToken)
        {
            try
            {
                var userId = Guid.Parse(this.GetClaim(ClaimTypes.NameIdentifier));

                await _unitOfWork.CommentRepository.Create(new Comment
                {
                    DatePosted = DateTime.UtcNow,
                    CreatorId = userId,
                    ScreenshotId = screenshotId,
                    Text = model.Text
                }, cancellationToken);

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);

                return Ok();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
