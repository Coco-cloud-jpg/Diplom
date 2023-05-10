using Common.Extensions;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using RecordingService.DTOS;

namespace ScreenMonitorService.Models
{
    public partial class ScreenContext : DbContext
    {
        public ScreenContext()
        {
        }

        public ScreenContext(DbContextOptions<ScreenContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<RecorderRegistration> RecorderRegistrations { get; set; } = null!;
        public virtual DbSet<Screenshot> Screenshots { get; set; } = null!;
        public virtual DbSet<RecorderRegistrationReadDTO> RecorderRegistrationDTOs { get; set; } = null!;
        public virtual DbSet<AppUsageDTO> AppUsageDTOs { get; set; } = null!;
        public virtual DbSet<CompanyUsersAndRecordersCountDTO> CompanyUsersAndRecordersCountDTOs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.AddCountryEntityBase();
                entity.ToTable("Countries");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.AddCompnayEntityBase();
                entity.ToTable("Companies");
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.AddUserEntityBase();
                entity.ToTable("Users");
            });


            modelBuilder.Entity<RecorderRegistration>(entity =>
            {
                entity.AddRecorderRegistrationEntityBase();
            });

            modelBuilder.Entity<Screenshot>(entity =>
            {
                entity.AddScreenshotEntityBase();
            });

            modelBuilder.Entity<Entry>(entity =>
            {
                entity.AddEntryEntityBase();
                entity.ToTable("Entries");
            });

            modelBuilder.Entity<PheripheralActivity>(entity =>
            {
                entity.AddPheripheralActivityEntityBase();
                entity.ToTable("PheripheralActivities");
            });


            modelBuilder.Entity<AlertRule>(entity =>
            {
                entity.AddAlertRuleEntityBase();
                entity.ToTable("AlertRules");
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

            modelBuilder.Entity<RecorderRegistrationReadDTO>().ToView(null);
            modelBuilder.Entity<AppUsageDTO>().ToView(null);
            modelBuilder.Entity<WeeklyReportDTO>().Ignore(item => item.DayOfWeekString).ToView(null);
            modelBuilder.Entity<ChartDTO>().ToView(null);
            modelBuilder.Entity<ChartEntranceDTO>().ToView(null);
            modelBuilder.Entity<CompanyUsersAndRecordersCountDTO>().ToView(null);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
