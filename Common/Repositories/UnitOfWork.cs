using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Common.Repositories.Repository
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        protected readonly DbContext _context;
        public BaseUnitOfWork(DbContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
