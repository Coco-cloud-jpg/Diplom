using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Models
{
    public partial class IdentityContext : DbContext
    {
        public IdentityContext()
        {
        }

        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.AddUserEntityBase();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.AddRefreshTokenEntityBase();
            });
            
            modelBuilder.Entity<Company>(entity =>
            {
                entity.AddCompnayEntityBase();
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
