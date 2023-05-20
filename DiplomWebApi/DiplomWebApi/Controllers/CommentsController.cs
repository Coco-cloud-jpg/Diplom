using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTOS;
using System.Security.Claims;
using BL.Services;

namespace DiplomWebApi.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentsService _commentsService;
        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }
        [HttpGet("{screenshotId}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetScreenshots(Guid screenshotId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _commentsService.GetScreenshots(screenshotId, cancellationToken));
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
                await _commentsService.Add(userId, screenshotId, model, cancellationToken);

                return Ok();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
