using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public DbContext Context { get; set; }
        public DbSet<TEntity> DbSet { get; set; }

        public GenericRepository(DbContext Context)
        {
            this.Context = Context;
            DbSet = Context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAll(CancellationToken Cancel)
        {
            return await DbSet.ToListAsync<TEntity>(Cancel);
        }
        public async Task<TEntity> GetById(Guid Id, CancellationToken Cancel)
        {
            return await DbSet.FirstOrDefaultAsync(Item => Item.Id == Id, Cancel) ?? throw new Exception();
        }
        public async Task Create(TEntity Element, CancellationToken Cancel)
        {
            if (Element == null)
            {
                throw new ArgumentNullException(nameof(Element));
            }
            await DbSet.AddAsync(Element, Cancel);
        }

        public async Task Delete(Guid Id, CancellationToken Cancel)
        {
            var Element = await DbSet.FirstOrDefaultAsync(Item => Item.Id == Id, Cancel);
            if (Element == null)
            {
                throw new ArgumentException(nameof(Id));
            }
            DbSet.Remove(Element);
        }

        public void Update(TEntity Element)
        {
            DbSet.Attach(Element);
            Context.Entry(Element).State = EntityState.Modified;
        }
    }
}
