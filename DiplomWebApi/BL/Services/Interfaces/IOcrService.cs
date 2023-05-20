using DAL.Interfaces;

namespace BL.Services.Interfaces
{
    public interface IOcrService
    {
        public Task Process(string base64, Guid companyId, Guid recorderId, Guid screenshotId, IScreenUnitOfWork _unitOfWork);
    }
}
