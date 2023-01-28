using Common.Interfaces;
using Common.Models;
using Common.Repositories.Repository;
using RecordingService.DTOS;
using ScreenMonitorService.Interfaces;
using ScreenMonitorService.Models;

namespace ScreenMonitorService.Repositories.Repository
{
    public class ScreenUnitOfWork : BaseUnitOfWork, IScreenUnitOfWork
    {
        private IGenericRepository<Company> _companyRepository;
        private IGenericRepository<RecorderRegistration> _recorderRegistrationRepository;
        private IGenericRepository<Screenshot> _screenshotRepository;
        private IGenericRepository<RecorderRegistrationReadDTO> _recorderRegistrationDTORepository;
        public ScreenUnitOfWork(ScreenContext context)
            : base(context)
        {
        }
        public IGenericRepository<RecorderRegistrationReadDTO> RecorderRegistrationDTORepository
        {
            get
            {
                if (this._recorderRegistrationDTORepository == null)
                {
                    this._recorderRegistrationDTORepository = new GenericRepository<RecorderRegistrationReadDTO>(_context);
                }
                return _recorderRegistrationDTORepository;
            }
        }
        public IGenericRepository<Company> CompanyRepository
        {
            get
            {
                if (this._companyRepository == null)
                {
                    this._companyRepository = new GenericRepository<Company>(_context);
                }
                return _companyRepository;
            }
        }
        public IGenericRepository<RecorderRegistration> RecorderRegistrationRepository
        {
            get
            {
                if (this._recorderRegistrationRepository == null)
                {
                    this._recorderRegistrationRepository = new GenericRepository<RecorderRegistration>(_context);
                }
                return _recorderRegistrationRepository;
            }
        }
        public IGenericRepository<Screenshot> ScreenshotRepository
        {
            get
            {
                if (this._screenshotRepository == null)
                {
                    this._screenshotRepository = new GenericRepository<Screenshot>(_context);
                }
                return _screenshotRepository;
            }
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
