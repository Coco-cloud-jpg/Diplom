using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void AddUserEntityBase(this EntityTypeBuilder<User> entity)
        {
            entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnName("firstName");

            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnName("lastnName");

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnName("email");

            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnName("password");

            entity.HasOne(e => e.Role).WithMany(e => e.Users).HasForeignKey(e => e.RoleId).IsRequired();
            entity.HasOne(e => e.Company).WithMany(e => e.Users).HasForeignKey(e => e.CompanyId).IsRequired(false);
            entity.HasOne(e => e.PasswordReset).WithOne(e => e.User).HasForeignKey<User>(e => e.PasswordResetId).IsRequired(false);
        }
        public static void AddRefreshTokenEntityBase(this EntityTypeBuilder<RefreshToken> entity)
        {
            entity.Property(e => e.Token).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.User).WithMany(e => e.RefreshTokens).HasForeignKey(e => e.UserId);
        }
        public static void AddCompnayEntityBase(this EntityTypeBuilder<Company> entity)
        {
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnName("name");

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnName("email");

            entity.HasOne(e => e.Country).WithMany(e => e.Companies).HasForeignKey(e => e.CountryId).IsRequired();
        }

        public static void AddRecorderRegistrationEntityBase(this EntityTypeBuilder<RecorderRegistration> entity)
        {
            entity.HasOne(e => e.Company).WithMany(c => c.RecorderRegistrations).HasForeignKey(e => e.CompanyId).IsRequired();
        }
        public static void AddScreenshotEntityBase(this EntityTypeBuilder<Screenshot> entity)
        {
            entity.HasOne(e => e.Recorder).WithMany(c => c.Screenshots).HasForeignKey(e => e.RecorderId).IsRequired();
        }
        public static void AddCountryEntityBase(this EntityTypeBuilder<Country> entity)
        {
            entity.Property(e => e.Name).HasMaxLength(40);
        }
        public static void AddPasswordResetEntityBase(this EntityTypeBuilder<PasswordReset> entity)
        {
            entity.HasOne(e => e.User).WithOne(e => e.PasswordReset).HasForeignKey<PasswordReset>(e => e.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        }

        public static void AddEntryEntityBase(this EntityTypeBuilder<Entry> entity)
        {
            entity.HasOne(item => item.Recorder).WithMany(item => item.Entries).HasForeignKey(item => item.RecorderId).IsRequired();
        }
        public static void AddPheripheralActivityEntityBase(this EntityTypeBuilder<PheripheralActivity> entity)
        {
            entity.HasOne(item => item.Recorder).WithMany(item => item.PheripheralActivites).HasForeignKey(item => item.RecorderId).IsRequired();
        }
        public static void AddAlertRuleEntityBase(this EntityTypeBuilder<AlertRule> entity)
        {
            entity.HasOne(item => item.Recorder).WithMany(item => item.AlertRules).HasForeignKey(item => item.RecorderId).IsRequired(false);
            entity.HasOne(item => item.Company).WithMany(item => item.AlertRules).HasForeignKey(item => item.CompanyId).IsRequired();
        }
        public static void AddApplicationUsageInfoEntityBase(this EntityTypeBuilder<ApplicationUsageInfo> entity)
        {
            entity.HasOne(item => item.Recorder).WithMany(item => item.ApplicationUsageInfo).HasForeignKey(item => item.RecorderId).IsRequired();
            entity.HasOne(item => item.ApplicationInfo).WithMany(item => item.ApplicationUsageInfo).HasForeignKey(item => item.ApplicationId).IsRequired();
        }

        public static void AddCommentEntityBase(this EntityTypeBuilder<Comment> entity)
        {
            entity.HasOne(item => item.Screenshot).WithMany(item => item.Comments).HasForeignKey(item => item.ScreenshotId).IsRequired();
            entity.HasOne(item => item.User).WithMany(item => item.Comments).HasForeignKey(item => item.CreatorId).IsRequired();
        }

        public static void AddPackageTypeCompanyEntityBase(this EntityTypeBuilder<PackageTypeCompany> entity)
        {
            entity.HasOne(item => item.Company).WithMany(item => item.PackageTypeCompanies).HasForeignKey(item => item.CompanyId).IsRequired();
            entity.HasOne(item => item.PackageType).WithMany(item => item.PackageTypeCompanies).HasForeignKey(item => item.PackageTypeId).IsRequired();
        }
        public static void AddBillingTransactionEntityBase(this EntityTypeBuilder<BillingTransaction> entity)
        {
            entity.HasOne(item => item.Company).WithMany(item => item.BillingTransactions).HasForeignKey(item => item.CompanyId).IsRequired();
        }
        public static void AddPackageTypeUpgradeRequests(this EntityTypeBuilder<PackageUpgradeRequest> entity)
        {
            entity.HasOne(item => item.Company).WithMany(item => item.PackageUpgradeRequests).HasForeignKey(item => item.CompanyId).IsRequired();
            entity.HasOne(item => item.PackageType).WithMany(item => item.PackageUpgradeRequests).HasForeignKey(item => item.PackageTypeId).IsRequired();
        }
    }
}
