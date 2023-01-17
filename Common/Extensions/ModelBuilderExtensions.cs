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

            entity.HasOne(e => e.Role).WithMany(item => item.Users).HasForeignKey(item => item.RoleId);
            entity.HasOne(e => e.Company).WithMany(item => item.Users).HasForeignKey(item => item.CompanyId);
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
        }
    }
}
