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
        private IGenericRepository<Entry> _entryRepository;
        private IGenericRepository<PheripheralActivity> _pheripheralActivityRepository;
        private IGenericRepository<ApplicationInfo> _applicationInfoRepository;
        private IGenericRepository<ApplicationUsageInfo> _applicationUsageInfoRepository;
        private IGenericRepository<AppUsageDTO> _appUsageDTORepository;
        private IGenericRepository<WeeklyReportDTO> _weeklyReportDTORepository;
        private IGenericRepository<ChartDTO> _chartDTORepository;
        private IGenericRepository<ChartEntranceDTO> _chartEntranceDTORepository;
        private IGenericRepository<AlertRule> _alertRuleRepository;
        public ScreenUnitOfWork(ScreenContext context)
            : base(context)
        {
        }
        public IGenericRepository<AlertRule> AlertRuleRepository
        {
            get
            {
                if (this._alertRuleRepository == null)
                {
                    this._alertRuleRepository = new GenericRepository<AlertRule>(_context);
                }
                return _alertRuleRepository;
            }
        }
        public IGenericRepository<ChartEntranceDTO> ChartEntranceDTORepository
        {
            get
            {
                if (this._chartEntranceDTORepository == null)
                {
                    this._chartEntranceDTORepository = new GenericRepository<ChartEntranceDTO>(_context);
                }
                return _chartEntranceDTORepository;
            }
        }
        public IGenericRepository<ChartDTO> ChartDTORepository
        {
            get
            {
                if (this._chartDTORepository == null)
                {
                    this._chartDTORepository = new GenericRepository<ChartDTO>(_context);
                }
                return _chartDTORepository;
            }
        }
        public IGenericRepository<WeeklyReportDTO> WeeklyReportDTORepository
        {
            get
            {
                if (this._weeklyReportDTORepository == null)
                {
                    this._weeklyReportDTORepository = new GenericRepository<WeeklyReportDTO>(_context);
                }
                return _weeklyReportDTORepository;
            }
        }
        public IGenericRepository<AppUsageDTO> AppUsageDTORepository
        {
            get
            {
                if (this._appUsageDTORepository == null)
                {
                    this._appUsageDTORepository = new GenericRepository<AppUsageDTO>(_context);
                }
                return _appUsageDTORepository;
            }
        }
        public IGenericRepository<ApplicationUsageInfo> ApplicationUsageInfoRepository
        {
            get
            {
                if (this._applicationUsageInfoRepository == null)
                {
                    this._applicationUsageInfoRepository = new GenericRepository<ApplicationUsageInfo>(_context);
                }
                return _applicationUsageInfoRepository;
            }
        }
        public IGenericRepository<ApplicationInfo> ApplicationInfoRepository
        {
            get
            {
                if (this._applicationInfoRepository == null)
                {
                    this._applicationInfoRepository = new GenericRepository<ApplicationInfo>(_context);
                }
                return _applicationInfoRepository;
            }
        }
        public IGenericRepository<PheripheralActivity> PheripheralActivityRepository
        {
            get
            {
                if (this._pheripheralActivityRepository == null)
                {
                    this._pheripheralActivityRepository = new GenericRepository<PheripheralActivity>(_context);
                }
                return _pheripheralActivityRepository;
            }
        }
        public IGenericRepository<Entry> EntryRepository
        {
            get
            {
                if (this._entryRepository == null)
                {
                    this._entryRepository = new GenericRepository<Entry>(_context);
                }
                return _entryRepository;
            }
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
