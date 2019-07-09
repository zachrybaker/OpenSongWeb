﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenSongWeb.Data;

namespace OpenSongWeb.Migrations
{
    [DbContext(typeof(SongDbContext))]
    partial class SongDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("OpenSongWeb.Data.AppConfiguration", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("Name");

                    b.ToTable("AppConfigurations");
                });

            modelBuilder.Entity("OpenSongWeb.Data.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DOB");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("OpenSongWeb.Data.AppUserRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("OpenSongWeb.Data.OSSong", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CCLINumber")
                        .HasMaxLength(12);

                    b.Property<int>("Capo");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("Copyright")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("''")
                        .HasMaxLength(255);

                    b.Property<string>("CreatedByID");

                    b.Property<DateTime>("CreatedDateUTC")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("HymnNumber")
                        .HasMaxLength(10);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<DateTime?>("LastUpdatedDateUTC")
                        .ValueGeneratedOnUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Presentation")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Tempo")
                        .HasMaxLength(120);

                    b.Property<string>("Themes");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("VideoEmbedID")
                        .HasMaxLength(20);

                    b.Property<string>("VideoLinkType")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("Author");

                    b.HasIndex("CreatedByID");

                    b.HasIndex("CreatedDateUTC");

                    b.HasIndex("Filename")
                        .IsUnique();

                    b.HasIndex("Title");

                    b.HasIndex("Title", "Author")
                        .IsUnique();

                    b.ToTable("OSSongs");
                });

            modelBuilder.Entity("OpenSongWeb.Data.SetEntry", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capo");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<int>("OSSongID");

                    b.Property<int>("Order");

                    b.Property<string>("Presentation")
                        .HasMaxLength(100);

                    b.Property<long>("SongSetID");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.HasIndex("OSSongID");

                    b.HasIndex("SongSetID");

                    b.ToTable("SetEntries");
                });

            modelBuilder.Entity("OpenSongWeb.Data.SetType", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TypicalAudienceSize");

                    b.HasKey("ID");

                    b.ToTable("SetTypes");
                });

            modelBuilder.Entity("OpenSongWeb.Data.SongSet", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedByEmail")
                        .HasMaxLength(255);

                    b.Property<Guid>("CreatedByID");

                    b.Property<string>("CreatedByName")
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedDateUTC");

                    b.Property<DateTime?>("EventDateUTC");

                    b.Property<bool>("IsPublic");

                    b.Property<DateTime?>("LastUpdatedDateUTC");

                    b.Property<string>("Notes");

                    b.Property<Guid?>("SetTypeId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.HasIndex("SetTypeId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("OpenSongWeb.Data.AppUserRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OpenSongWeb.Data.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OpenSongWeb.Data.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("OpenSongWeb.Data.AppUserRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OpenSongWeb.Data.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("OpenSongWeb.Data.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OpenSongWeb.Data.OSSong", b =>
                {
                    b.HasOne("OpenSongWeb.Data.AppUser", "CreatedBy")
                        .WithMany("SongsCreated")
                        .HasForeignKey("CreatedByID");
                });

            modelBuilder.Entity("OpenSongWeb.Data.SetEntry", b =>
                {
                    b.HasOne("OpenSongWeb.Data.OSSong", "OSSong")
                        .WithMany()
                        .HasForeignKey("OSSongID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OpenSongWeb.Data.SongSet", "SongSet")
                        .WithMany("SetEntries")
                        .HasForeignKey("SongSetID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OpenSongWeb.Data.SongSet", b =>
                {
                    b.HasOne("OpenSongWeb.Data.SetType", "SetType")
                        .WithMany()
                        .HasForeignKey("SetTypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
