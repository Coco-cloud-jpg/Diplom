using Common.Models;
using DAL.DTOS;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;

namespace BL.Services
{
    public class AlertsService: IAlertsService
    {
        private IScreenUnitOfWork _unitOfWork;
        public AlertsService(IScreenUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ShortEntityModelGuid>> GetRecordersInfo(Guid companyId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.RecorderRegistrationRepository
                .DbSet.AsNoTracking().Where(item => item.CompanyId == companyId)
                .Select(item => new ShortEntityModelGuid { Id = item.Id, Name = $"{item.HolderName} {item.HolderSurname}" }).ToListAsync(cancellationToken);
        }
        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.AlertRuleRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        public async Task Create(Guid companyId, AlertRuleCreateDTO model, CancellationToken cancellationToken)
        {
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
        }
        public async Task Update(Guid id, AlertRuleCreateDTO model, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _unitOfWork.AlertRuleRepository.GetById(id, cancellationToken);

            if (itemToUpdate == null)
                throw new ArgumentNullException();

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
        }
        public async Task<List<AlertRuleReadDTO>> GetAllRules(Guid companyId, int page, int pageSize, CancellationToken cancellationToken) =>
            await _unitOfWork.AlertRuleRepository.DbSet.Include(item => item.Recorder)
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
                .ToListAsync(cancellationToken);

    }
}
