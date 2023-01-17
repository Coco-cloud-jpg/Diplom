using Common.Interfaces;
using Common.Repositories.Repository;
using ScreenMonitorService.Interfaces;
using ScreenMonitorService.Models;

namespace ScreenMonitorService.Repositories.Repository
{
    public class ScreenUnitOfWork : BaseUnitOfWork, IScreenUnitOfWork
    {
        private IGenericRepository<Customer> _customerRepository;
        private IGenericRepository<RecorderRegistration> _recorderRegistrationRepository;
        private IGenericRepository<Screenshot> _screenshotRepository;
        public ScreenUnitOfWork(ScreenContext context)
            : base(context)
        {
        }
        public IGenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (this._customerRepository == null)
                {
                    this._customerRepository = new GenericRepository<Customer>(_context);
                }
                return _customerRepository;
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
