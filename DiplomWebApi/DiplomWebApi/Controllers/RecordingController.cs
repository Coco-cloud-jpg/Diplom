using Common.Extensions;
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
            await _unitOfWork.CompanyRepository.GetAll(CancellationToken.None);
            var recorder = await _unitOfWork.RecorderRegistrationRepository.DbSet.FirstOrDefaultAsync(item => item.CompanyId == model.CompanyId && item.Id == model.RecorderId && item.IsActive);

            if (recorder == null)
                return Unauthorized();

            return Ok(recorder.Id);

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

        //[HttpPost]
        //[Authorize(Roles = $"{nameof(Common.Constants.Role.CompanyAdmin)},{nameof(Common.Constants.Role.User)}")]
        //public async Task<IActionResult> Create(RecorderRegistrationCreateDTO model)
        //{
        //    try
        //    {
        //        await _unitOfWork.RecorderRegistrationRepository.Delete(model.Id, CancellationToken.None);
        //        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        //        return NoContent();
        //    }
        //    catch (Exception e)
        //    {
        //        return NotFound();
        //    }
        //}
    }
}
