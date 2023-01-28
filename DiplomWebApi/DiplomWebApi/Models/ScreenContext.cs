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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.AddCompnayEntityBase();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.AddUserEntityBase();
            });

            modelBuilder.Entity<RecorderRegistration>(entity =>
            {
                entity.AddRecorderRegistrationEntityBase();
            });

            modelBuilder.Entity<Screenshot>(entity =>
            {
                entity.AddScreenshotEntityBase();
            });

            modelBuilder.Entity<RecorderRegistrationReadDTO>().ToView(null);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
