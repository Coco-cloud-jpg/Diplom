using Microsoft.EntityFrameworkCore;

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

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<RecorderRegistration> RecorderRegistrations { get; set; } = null!;
        public virtual DbSet<Screenshot> Screenshots { get; set; } = null!;

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-7MCGKLEC\\SQLEXPRESS;Database=FirstVersion;Trusted_Connection=True;");
            }
        }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RecorderRegistration>(entity =>
            {
                entity.HasIndex(e => e.MacAddress, "UQ__Recorder__B01A99EC45745763")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ApiKey).HasColumnName("apiKey");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.MacAddress)
                    .HasMaxLength(25)
                    .HasColumnName("macAddress");

                entity.Property(e => e.TimeCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("timeCreated");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RecorderRegistrations)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__RecorderR__custo__276EDEB3");
            });

            modelBuilder.Entity<Screenshot>(entity =>
            {
                entity.HasOne(item => item.Customer).WithMany(item => item.Screenshots).HasForeignKey(item => item.CustomerId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
