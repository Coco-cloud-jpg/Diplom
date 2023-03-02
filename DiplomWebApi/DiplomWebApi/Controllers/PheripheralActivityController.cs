using Common.Extensions;
using Common.Models;
using Common.Services.Interfaces;
using DiplomWebApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecordingService.DTOS;
using ScreenMonitorService.Interfaces;

namespace DiplomWebApi.Controllers
{
    [Route("api/pheripheral")]
    [ApiController]
    public class PheripheralActivityController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        public PheripheralActivityController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddEntry(PheripheralActivityDTO model)
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

            return Ok();
        }
    }
}
