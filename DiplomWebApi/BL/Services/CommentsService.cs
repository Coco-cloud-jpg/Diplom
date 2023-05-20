using Common.Models;
using DAL.DTOS;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CommentsService: ICommentsService
    {
        private IScreenUnitOfWork _unitOfWork;
        public CommentsService(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CommentDTO>> GetScreenshots(Guid screenshotId, CancellationToken cancellationToken) =>
            await _unitOfWork.CommentRepository.DbSet
                    .Include(item => item.User)
                    .Where(item => item.ScreenshotId == screenshotId)
                    .OrderByDescending(item => item.DatePosted)
                    .Select(item => new CommentDTO
                    {
                        Text = item.Text,
                        DatePosted = item.DatePosted.ToString("g"),
                        PosterName = $"{item.User.FirstName} {item.User.LastName}"
                    })
                    .ToListAsync(cancellationToken);
        public async Task Add(Guid userId, Guid screenshotId, CommentAddDTO model, CancellationToken cancellationToken)
        {
            await _unitOfWork.CommentRepository.Create(new Comment
            {
                DatePosted = DateTime.UtcNow,
                CreatorId = userId,
                ScreenshotId = screenshotId,
                Text = model.Text
            }, cancellationToken);

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
