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


            modelBuilder.Entity<Entry>(entity =>
            {
                entity.AddEntryEntityBase();
                entity.ToTable("Entries");
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

            modelBuilder.Entity<PheripheralActivity>(entity =>
            {
                entity.AddPheripheralActivityEntityBase();
                entity.ToTable("PheripheralActivities");
            });

            modelBuilder.Entity<ApplicationInfo>(entity =>
            {
                entity.ToTable("ApplicationInfos");
            });

            modelBuilder.Entity<ApplicationUsageInfo>(entity =>
            {
                entity.AddApplicationUsageInfoEntityBase();
                entity.ToTable("ApplicationUsageInfos");
            });

            modelBuilder.Entity<AlertRule>(entity =>
            {
                entity.AddAlertRuleEntityBase();
                entity.ToTable("AlertRules");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.AddCommentEntityBase();
                entity.ToTable("Comments");
            });

            modelBuilder.Entity<PackageType>(entity =>
            {
                entity.ToTable("PackageTypes");
            });

            modelBuilder.Entity<PackageTypeCompany>(entity =>
            {
                entity.AddPackageTypeCompanyEntityBase();
                entity.ToTable("PackageTypeCompanies");
            });

            modelBuilder.Entity<BillingTransaction>(entity =>
            {
                entity.AddBillingTransactionEntityBase();
                entity.ToTable("BillingTransactions");
            });

            modelBuilder.Entity<PackageUpgradeRequest>(entity =>
            {
                entity.AddPackageTypeUpgradeRequests();
                entity.ToTable("PackageUpgradeRequests");
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
                    Password = "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817",
                    IsActive = true
                });

            modelBuilder.Entity<PackageType>().HasData(
                new PackageType
                {
                    Id = Guid.NewGuid(),
                    Name = "Basic USD",
                    MaxRecordersCount = 5,
                    MaxUsersCount = 2,
                    Price = 20,
                    Currency = (short)Currency.USD
                },
                new PackageType
                {
                    Id = Guid.NewGuid(),
                    Name = "Advanced USD",
                    MaxRecordersCount = 15,
                    MaxUsersCount = 8,
                    Price = 50,
                    Currency = (short)Currency.USD
                },
                new PackageType
                {
                    Id = Guid.NewGuid(),
                    Name = "High Capacity USD",
                    MaxRecordersCount = 25,
                    MaxUsersCount = 15,
                    Price = 80,
                    Currency = (short)Currency.USD
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
