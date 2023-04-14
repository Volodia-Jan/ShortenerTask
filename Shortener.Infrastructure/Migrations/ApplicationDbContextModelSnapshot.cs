﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shortener.Infrastructure.DbContext;

#nullable disable

namespace Shortener.Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "7.0.1")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("RoleId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderKey")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
            {
                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("RoleId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles", (string)null);

                b.HasData(
                    new
                    {
                        UserId = new Guid("2ea1e1f3-27a1-4140-b938-f1f2f2829478"),
                        RoleId = new Guid("2f32ee00-8974-4215-8fd8-07e0befaf890")
                    });
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
            {
                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Value")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens", (string)null);
            });

        modelBuilder.Entity("Shortener.Core.Entities.Post", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid?>("AuthorId")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("PostDetails")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Topic")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("AuthorId");

                b.ToTable("Posts", (string)null);
            });

        modelBuilder.Entity("Shortener.Core.Entities.Url", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("BaseUrl")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime?>("CreatedDate")
                    .HasColumnType("datetime2");

                b.Property<string>("ShortUrl")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("Urls", (string)null);
            });

        modelBuilder.Entity("Shortener.Core.IdentityEntities.ApplicationRole", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex")
                    .HasFilter("[NormalizedName] IS NOT NULL");

                b.ToTable("AspNetRoles", (string)null);

                b.HasData(
                    new
                    {
                        Id = new Guid("2f32ee00-8974-4215-8fd8-07e0befaf890"),
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new
                    {
                        Id = new Guid("b7612d57-bb44-4913-8c7f-8a6aba4fe3d5"),
                        Name = "Ordinal",
                        NormalizedName = "ORDINAL"
                    });
            });

        modelBuilder.Entity("Shortener.Core.IdentityEntities.ApplicationUser", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("int");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("bit");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("bit");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("datetimeoffset");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PersonName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("bit");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("bit");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex")
                    .HasFilter("[NormalizedUserName] IS NOT NULL");

                b.ToTable("AspNetUsers", (string)null);

                b.HasData(
                    new
                    {
                        Id = new Guid("2ea1e1f3-27a1-4140-b938-f1f2f2829478"),
                        AccessFailedCount = 0,
                        ConcurrencyStamp = "8775a6e5-a388-4848-b1cf-492be8909fbf",
                        Email = "admin@example.com",
                        EmailConfirmed = false,
                        LockoutEnabled = false,
                        NormalizedEmail = "ADMIN@EXAMPLE.COM",
                        NormalizedUserName = "ADMIN@EXAMPLE.COM",
                        PasswordHash = "AQAAAAIAAYagAAAAEFKXHAZdGiv12S7FZFVEaoDxigGuOB3Z/3+zcBFUQAif7CqEywS7esWmjPEKV64wqQ==",
                        PersonName = "Admin",
                        PhoneNumber = "1234567890",
                        PhoneNumberConfirmed = false,
                        SecurityStamp = "7a2581b5-c199-4502-964a-70c02c6b1d34",
                        TwoFactorEnabled = false,
                        UserName = "admin@example.com"
                    });
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
            {
                b.HasOne("Shortener.Core.IdentityEntities.ApplicationRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
            {
                b.HasOne("Shortener.Core.IdentityEntities.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
            {
                b.HasOne("Shortener.Core.IdentityEntities.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
            {
                b.HasOne("Shortener.Core.IdentityEntities.ApplicationRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Shortener.Core.IdentityEntities.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
            {
                b.HasOne("Shortener.Core.IdentityEntities.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Shortener.Core.Entities.Post", b =>
            {
                b.HasOne("Shortener.Core.IdentityEntities.ApplicationUser", "User")
                    .WithMany()
                    .HasForeignKey("AuthorId");

                b.Navigation("User");
            });

        modelBuilder.Entity("Shortener.Core.Entities.Url", b =>
            {
                b.HasOne("Shortener.Core.IdentityEntities.ApplicationUser", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("User");
            });
#pragma warning restore 612, 618
    }
}
