using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;
using ScreenMonitorService.Interfaces;

namespace DiplomWebApi.Controllers
{
    [Route("api/alerts")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private IScreenUnitOfWork _unitOfWork;
        public AlertsController(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("recorders")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetRecordersInfo(CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            return Ok(await _unitOfWork.RecorderRegistrationRepository
                .DbSet.AsNoTracking().Where(item => item.CompanyId == companyId)
                .Select(item => new { Id = item.Id, Name = $"{item.HolderName} {item.HolderSurname}" }).ToListAsync(cancellationToken));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            await _unitOfWork.AlertRuleRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Create(AlertRuleCreateDTO model, CancellationToken cancellationToken)
        {
            var companyId = Guid.Parse(this.GetClaim("CompanyId"));

            var trimmeredWords = new List<string>();

            foreach (var item in model.SerializedWords.Trim().Split(","))
            {
                trimmeredWords.Add(item.Trim());
            }

            var dbModel = new AlertRule
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                RecorderId = model.RecorderId,
                SendToEmail = model.SendToEmail,
                SerializedWords = $"[\"{String.Join("\",\"", trimmeredWords)}\"]",
                DateCreated = DateTime.Now
            };

            await _unitOfWork.AlertRuleRepository.Create(dbModel, cancellationToken);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return Ok();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> Update(Guid id, AlertRuleCreateDTO model, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _unitOfWork.AlertRuleRepository.GetById(id, cancellationToken);

            if (itemToUpdate == null)
                return NotFound();

            var trimmeredWords = new List<string>();

            foreach (var item in model.SerializedWords.Trim().Split(","))
            {
                trimmeredWords.Add(item.Trim());
            }

            itemToUpdate.SendToEmail = model.SendToEmail;
            itemToUpdate.SerializedWords = $"[\"{String.Join("\",\"", trimmeredWords)}\"]";
            itemToUpdate.RecorderId = model.RecorderId;

            _unitOfWork.AlertRuleRepository.Update(itemToUpdate);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return Ok();
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        public async Task<IActionResult> GetAllRules(int page, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var companyId = Guid.Parse(this.GetClaim("CompanyId"));

                return Ok(await _unitOfWork.AlertRuleRepository.DbSet.Include(item => item.Recorder)
                    .Where(item => item.CompanyId == companyId)
                    .Select(item => new AlertRuleReadDTO
                    {
                        Id = item.Id,
                        SendToEmail = item.SendToEmail,
                        SerializedWords = item.SerializedWords,
                        DateCreated = item.DateCreated,
                        ToRecorder = $"{item.Recorder.HolderName} {item.Recorder.HolderSurname}"
                    })
                    .OrderByDescending(item => item.DateCreated).Skip(page * pageSize).Take(pageSize)
                    .ToListAsync(cancellationToken));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
