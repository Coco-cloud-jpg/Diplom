using Common.Models;
using Common.Services.Interfaces;
using DAL;
using DAL.DTOS;
using DiplomWebApi.DTOS;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class RecordingService: IRecordingService
    {
        private IScreenUnitOfWork _unitOfWork;
        private IBlobService _blobService;
        public RecordingService(IScreenUnitOfWork unitOfWork, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }
        public async Task<Guid> Authorize(RecorderAuthorizationDTO model)
        {
            var recorder = await _unitOfWork.RecorderRegistrationRepository.DbSet.FirstOrDefaultAsync(item => item.CompanyId == model.CompanyId && item.Id == model.RecorderId && item.IsActive);

            if (recorder == null)
                throw new UnauthorizedAccessException();

            return recorder.Id;

        }

        public async Task Create(Guid companyId, RecorderRegistrationDTO model)
        {
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
        }

        public async Task<List<RecorderRegistrationReadDTO>> GetAllRecorders(Guid companyId, bool includeDeleted, CancellationToken cancellationToken) =>
            await _unitOfWork.RecorderRegistrationDTORepository.DbSet.FromSqlRaw($"exec {StoredProcedures.GetRecordersInfo} @includeDeleted = @includeDeleted, @companyId = @companyId",
                    new SqlParameter("@includeDeleted", includeDeleted),
                    new SqlParameter("@companyId", companyId)
                    ).ToListAsync(cancellationToken);
        public async Task<RecorderDetailsTodayDTO> GetToday(Guid id)
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

            return new RecorderDetailsTodayDTO
            {
                Screenshots = screenshotsCount,
                MouseActivity = mouseActivityPercent,
                KeyboardActivity = keyboardActivityPercent
            };
        }
        public async Task Activate(Guid id, bool activeState)
        {
            var item = await _unitOfWork.RecorderRegistrationRepository.GetById(id, CancellationToken.None);

            if (item == null)
                throw new ArgumentNullException();

            item.IsActive = activeState;

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
