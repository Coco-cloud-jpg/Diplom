using DAL.DTOS;

namespace BL.Services
{
    public interface IScreenshotTakeService
    {
        Task AddScreenShot(ScreenshotCreateDTO model);
    }
}
