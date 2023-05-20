﻿// <auto-generated />
using System;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Identity.Migrations
{
    [DbContext(typeof(IdentityContext))]
    [Migration("20230311194632_AddComment")]
    partial class AddComment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Common.Models.AlertRule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("RecorderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SendToEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerializedWords")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("RecorderId");

                    b.ToTable("AlertRules", (string)null);
                });

            modelBuilder.Entity("Common.Models.ApplicationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IconBase64")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationInfos", (string)null);
                });

            modelBuilder.Entity("Common.Models.ApplicationUsageInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RecorderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Seconds")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("RecorderId");

                    b.ToTable("ApplicationUsageInfos", (string)null);
                });

            modelBuilder.Entity("Common.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ScreenshotId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ScreenshotId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("Common.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("email");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Companies", (string)null);
                });

            modelBuilder.Entity("Common.Models.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Countries", (string)null);
                });

            modelBuilder.Entity("Common.Models.Entry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RecorderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Seconds")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RecorderId");

                    b.ToTable("Entries", (string)null);
                });

            modelBuilder.Entity("Common.Models.PasswordReset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("PasswordResets", (string)null);
                });

            modelBuilder.Entity("Common.Models.PheripheralActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<double>("KeyboardActivePercentage")
                        .HasColumnType("float");

                    b.Property<double>("MouseActivePercentage")
                        .HasColumnType("float");

                    b.Property<Guid>("RecorderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RecorderId");

                    b.ToTable("PheripheralActivities", (string)null);
                });

            modelBuilder.Entity("Common.Models.RecorderRegistration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HolderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HolderSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("RecorderRegistrations", (string)null);
                });

            modelBuilder.Entity("Common.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ValidUntil")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("Common.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3"),
                            Name = "SystemAdmin"
                        },
                        new
                        {
                            Id = new Guid("b458e703-4168-446e-a163-63d0f9034d93"),
                            Name = "User"
                        },
                        new
                        {
                            Id = new Guid("00001457-f277-4ca9-9cf0-63d0f9034d93"),
                            Name = "CompanyAdmin"
                        });
                });

            modelBuilder.Entity("Common.Models.Screenshot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("Mark")
                        .HasColumnType("int");

                    b.Property<Guid>("RecorderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StorePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RecorderId");

                    b.ToTable("Screenshots", (string)null);
                });

            modelBuilder.Entity("Common.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("firstName");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("lastnName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("password");

                    b.Property<Guid?>("PasswordResetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("5e633f07-0b8d-40d3-a6b2-20f15cf09a0d"),
                            Email = "palya1703@gmail.com",
                            FirstName = "Pavlo",
                            IsActive = false,
                            LastName = "Koval",
                            Password = "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817",
                            RoleId = new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3")
                        });
                });

            modelBuilder.Entity("Common.Models.AlertRule", b =>
                {
                    b.HasOne("Common.Models.Company", "Company")
                        .WithMany("AlertRules")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Common.Models.RecorderRegistration", "Recorder")
                        .WithMany("AlertRules")
                        .HasForeignKey("RecorderId");

                    b.Navigation("Company");

                    b.Navigation("Recorder");
                });

            modelBuilder.Entity("Common.Models.ApplicationUsageInfo", b =>
                {
                    b.HasOne("Common.Models.ApplicationInfo", "ApplicationInfo")
                        .WithMany("ApplicationUsageInfo")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Common.Models.RecorderRegistration", "Recorder")
                        .WithMany("ApplicationUsageInfo")
                        .HasForeignKey("RecorderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationInfo");

                    b.Navigation("Recorder");
                });

            modelBuilder.Entity("Common.Models.Comment", b =>
                {
                    b.HasOne("Common.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Common.Models.Screenshot", "Screenshot")
                        .WithMany("Comments")
                        .HasForeignKey("ScreenshotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Screenshot");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Common.Models.Company", b =>
                {
                    b.HasOne("Common.Models.Country", "Country")
                        .WithMany("Companies")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Common.Models.Entry", b =>
                {
                    b.HasOne("Common.Models.RecorderRegistration", "Recorder")
                        .WithMany("Entries")
                        .HasForeignKey("RecorderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recorder");
                });

            modelBuilder.Entity("Common.Models.PasswordReset", b =>
                {
                    b.HasOne("Common.Models.User", "User")
                        .WithOne("PasswordReset")
                        .HasForeignKey("Common.Models.PasswordReset", "UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Common.Models.PheripheralActivity", b =>
                {
                    b.HasOne("Common.Models.RecorderRegistration", "Recorder")
                        .WithMany("PheripheralActivites")
                        .HasForeignKey("RecorderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recorder");
                });

            modelBuilder.Entity("Common.Models.RecorderRegistration", b =>
                {
                    b.HasOne("Common.Models.Company", "Company")
                        .WithMany("RecorderRegistrations")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Common.Models.RefreshToken", b =>
                {
                    b.HasOne("Common.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Common.Models.Screenshot", b =>
                {
                    b.HasOne("Common.Models.RecorderRegistration", "Recorder")
                        .WithMany("Screenshots")
                        .HasForeignKey("RecorderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recorder");
                });

            modelBuilder.Entity("Common.Models.User", b =>
                {
                    b.HasOne("Common.Models.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId");

                    b.HasOne("Common.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Common.Models.ApplicationInfo", b =>
                {
                    b.Navigation("ApplicationUsageInfo");
                });

            modelBuilder.Entity("Common.Models.Company", b =>
                {
                    b.Navigation("AlertRules");

                    b.Navigation("RecorderRegistrations");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Common.Models.Country", b =>
                {
                    b.Navigation("Companies");
                });

            modelBuilder.Entity("Common.Models.RecorderRegistration", b =>
                {
                    b.Navigation("AlertRules");

                    b.Navigation("ApplicationUsageInfo");

                    b.Navigation("Entries");

                    b.Navigation("PheripheralActivites");

                    b.Navigation("Screenshots");
                });

            modelBuilder.Entity("Common.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Common.Models.Screenshot", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Common.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("PasswordReset");

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
