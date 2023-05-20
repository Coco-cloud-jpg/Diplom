using Common.Models;
using DAL.DTOS;
using DAL.Interfaces;

namespace BL.Services
{
    public class PheripheralActivityService: IPheripheralActivityService
    {
        private IScreenUnitOfWork _unitOfWork;
        public PheripheralActivityService(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddEntry(PheripheralActivityDTO model)
        {
            await _unitOfWork.PheripheralActivityRepository.Create(new PheripheralActivity
            {
                Id = Guid.NewGuid(),
                RecorderId = model.RecorderId,
                MouseActivePercentage = model.MouseActivity,
                KeyboardActivePercentage = model.KeyboardActivity,
                DateCreated = DateTime.UtcNow
            }, CancellationToken.None);

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
