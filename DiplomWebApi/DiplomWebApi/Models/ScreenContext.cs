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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.AddCompnayEntityBase();
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

            modelBuilder.Entity<RecorderRegistrationReadDTO>().ToView(null);
            modelBuilder.Entity<AppUsageDTO>().ToView(null);
            modelBuilder.Entity<WeeklyReportDTO>().Ignore(item => item.DayOfWeekString).ToView(null);
            modelBuilder.Entity<ChartDTO>().ToView(null);
            modelBuilder.Entity<ChartEntranceDTO>().ToView(null);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
