using Common.Extensions;
using Common.Models;
using Common.Services.Interfaces;
using DiplomWebApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RecordingService;
using RecordingService.DTOS;
using ScreenMonitorService.Interfaces;
using System.Security.Claims;

namespace DiplomWebApi.Controllers
{
    [Route("api/recordings")]
    [ApiController]
    public class RecordingController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        private IBlobService _blobService;
        public RecordingController(IScreenUnitOfWork unitOfWork, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }

        [HttpPost("authorize")]
        [AllowAnonymous]
        public async Task<IActionResult> Authorize(RecorderAuthorizationDTO model)
        {
            var recorder = await _unitOfWork.RecorderRegistrationRepository.DbSet.FirstOrDefaultAsync(item => item.CompanyId == model.CompanyId && item.Id == model.RecorderId && item.IsActive);

            if (recorder == null)
                return Unauthorized();

            return Ok(recorder.Id);

        }
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Create(RecorderRegistrationDTO model)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                await _unitOfWork.RecorderRegistrationRepository.Create(new RecorderRegistration
                {
                    Id = Guid.NewGuid(),
                    CompanyId = companyId,
                    HolderName = model.HolderName,
                    HolderSurname = model.HolderSurname,
                    IsActive = true,
                    TimeCreated = DateTime.UtcNow
                }, CancellationToken.None);

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetAllRecorders(bool includeDeleted, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                return Ok(await _unitOfWork.RecorderRegistrationDTORepository.DbSet.FromSqlRaw($"exec {StoredProcedures.GetRecordersInfo} @includeDeleted = @includeDeleted, @companyId = @companyId",
                    new SqlParameter("@includeDeleted", includeDeleted),
                    new SqlParameter("@companyId", companyId)
                    ).ToListAsync(cancellationToken));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetToday(Guid id)
        {
            try
            {
                var todayStart = DateTime.UtcNow.Date;
                var pheripheralActivities = await _unitOfWork.PheripheralActivityRepository.DbSet
                    .Where(item => item.RecorderId == id && item.DateCreated > todayStart).ToListAsync();

                var quantity = pheripheralActivities.Count;
                var mouseActivityPercent = 0.0;
                var keyboardActivityPercent = 0.0;

                if (quantity != 0)
                {
                    mouseActivityPercent = pheripheralActivities.Sum(item => item.MouseActivePercentage) / quantity;
                    keyboardActivityPercent = pheripheralActivities.Sum(item => item.KeyboardActivePercentage) / quantity;
                }

                var screenshotsCount = await _unitOfWork.ScreenshotRepository.DbSet
                    .Where(item => item.RecorderId == id && item.DateCreated > todayStart).CountAsync();

                return Ok(new RecorderDetailsTodayDTO
                {
                    Screenshots = screenshotsCount,
                    MouseActivity = mouseActivityPercent,
                    KeyboardActivity = keyboardActivityPercent
                });
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpPatch("activate/{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Activate(Guid id, bool activeState)
        {
            try
            {
                var item = await _unitOfWork.RecorderRegistrationRepository.GetById(id, CancellationToken.None);

                if (item == null)
                    return NotFound();

                item.IsActive = activeState;

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);

                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
