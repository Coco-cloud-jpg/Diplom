using DAL.DTOS;

namespace BL.Services
{
    public interface IWarningsService
    {
        Task Add(Guid userId, Guid screenshotId, WarningCommentAddDTO model, CancellationToken cancellationToken);
        Task<GridResult<ScreenshotWarning>> GetAllWarnings(Guid companyId, int page, CancellationToken cancellationToken);
    }
}
