using Common.Models;
using DAL.DTOS;
using Microsoft.EntityFrameworkCore;

namespace BL.Services
{
    public interface ICommentsService
    {
        Task<List<CommentDTO>> GetScreenshots(Guid screenshotId, CancellationToken cancellationToken);
        Task Add(Guid userId, Guid screenshotId, CommentAddDTO model, CancellationToken cancellationToken);
    }
}
