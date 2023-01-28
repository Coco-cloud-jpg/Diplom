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
                entity.ToTable("Users");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.AddRefreshTokenEntityBase();
                entity.ToTable("RefreshTokens");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.AddCompnayEntityBase();
                entity.ToTable("Companies");
            });

            modelBuilder.Entity<RecorderRegistration>(entity =>
            {
                entity.AddRecorderRegistrationEntityBase();
                entity.ToTable("RecorderRegistrations");
            });

            modelBuilder.Entity<Screenshot>(entity =>
            {
                entity.AddScreenshotEntityBase();
                entity.ToTable("Screenshots");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.AddCountryEntityBase();
                entity.ToTable("Countries");
            });

            modelBuilder.Entity<PasswordReset>(entity =>
            {
                entity.AddPasswordResetEntityBase();
                entity.ToTable("PasswordResets");
            });

            modelBuilder.Entity<Role>().HasData(new Role
            {
                Name = nameof(Common.Constants.Role.SystemAdmin),
                Id = Guid.Parse(Common.Constants.Role.SystemAdmin),
            }, new Role
            {
                Name = nameof(Common.Constants.Role.User),
                Id = Guid.Parse(Common.Constants.Role.User),
            }, new Role
            {
                Name = nameof(Common.Constants.Role.CompanyAdmin),
                Id = Guid.Parse(Common.Constants.Role.CompanyAdmin),
            });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    CompanyId = null,
                    FirstName = "Pavlo",
                    LastName = "Koval",
                    Email = "palya1703@gmail.com",
                    RoleId = Guid.Parse(Common.Constants.Role.SystemAdmin),
                    Password = "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817"
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
