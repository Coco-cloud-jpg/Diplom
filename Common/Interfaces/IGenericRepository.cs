using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public DbContext Context { get; set; }
        public DbSet<TEntity> DbSet { get; set; }
        Task<IEnumerable<TEntity>> GetAll(CancellationToken Cancel);
        Task<TEntity> GetById(Guid Id, CancellationToken Cancel);
        Task Create(TEntity Element, CancellationToken Cancel);
        Task Delete(Guid Id, CancellationToken Cancel);
        void Update(TEntity Element);
    }
}
