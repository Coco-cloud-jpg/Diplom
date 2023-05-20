using BL.Extensions;
using Common.Models;
using Common.Services.Interfaces;
using DAL.DTOS;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

namespace BL.Services
{
    public class WarningsService: IWarningsService
    {
        private IScreenUnitOfWork _unitOfWork;
        private IBlobService _blobService;
        public WarningsService(IScreenUnitOfWork unitOfWork, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }
        public async Task Add(Guid userId, Guid screenshotId, WarningCommentAddDTO model, CancellationToken cancellationToken)
        {
            var screenshot = await _unitOfWork.ScreenshotRepository.GetById(screenshotId, cancellationToken);

            if (screenshot == null)
                throw new ArgumentNullException();

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
        }
        public async Task<GridResult<ScreenshotWarning>> GetAllWarnings(Guid companyId, int page, CancellationToken cancellationToken)
        {
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
                throw new ArgumentNullException();

            screenshotEntry.Base64 = $"data:image/jpeg;base64,{(await _blobService.GetBlobAsync("screenshots", screenshotEntry.Path)).ToBase64String()}";

            return new GridResult<ScreenshotWarning>
            {
                Data = new List<ScreenshotWarning> { screenshotEntry },
                Total = await totalTask
            };
        }
    }
}
