﻿// <auto-generated />
using System;
using App.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.DAL.EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("App.Domain.Content", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContentTypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LanguageStringId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("NewsId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ContentTypeId");

                    b.HasIndex("LanguageStringId")
                        .IsUnique();

                    b.HasIndex("NewsId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Contents");
                });

            modelBuilder.Entity("App.Domain.ContentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ContentTypes");
                });

            modelBuilder.Entity("App.Domain.HasTopicArea", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("NewsId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TopicAreaId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("NewsId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TopicAreaId");

                    b.ToTable("HasTopicAreas");
                });

            modelBuilder.Entity("App.Domain.Identity.AppRefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpirtationDT")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("AppRefreshTokens");
                });

            modelBuilder.Entity("App.Domain.Identity.AppRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AppUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("App.Domain.Identity.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("App.Domain.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("App.Domain.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<double>("PriceVolume")
                        .HasColumnType("double precision");

                    b.Property<string>("ProjectManager")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ThumbnailImage")
                        .HasColumnType("text");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("App.Domain.TopicArea", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("LanguageStringId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ParentTopicAreaId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LanguageStringId")
                        .IsUnique();

                    b.HasIndex("ParentTopicAreaId");

                    b.ToTable("TopicAreas");
                });

            modelBuilder.Entity("App.Domain.Translations.LanguageString", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ContentId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TopicAreaId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique()
                        .HasFilter("\"TopicAreaId\" IS NOT NULL");

                    b.ToTable("LanguageStrings");
                });

            modelBuilder.Entity("App.Domain.Translations.LanguageStringTranslation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("LanguageCulture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LanguageStringId")
                        .HasColumnType("uuid");

                    b.Property<string>("TranslationValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LanguageStringId");

                    b.ToTable("LanguageStringTranslations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("App.Domain.Content", b =>
                {
                    b.HasOne("App.Domain.ContentType", "ContentType")
                        .WithMany()
                        .HasForeignKey("ContentTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("App.Domain.Translations.LanguageString", "LanguageString")
                        .WithOne("Content")
                        .HasForeignKey("App.Domain.Content", "LanguageStringId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("App.Domain.News", "News")
                        .WithMany("Content")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("App.Domain.Project", "Project")
                        .WithMany("Content")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ContentType");

                    b.Navigation("LanguageString");

                    b.Navigation("News");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("App.Domain.HasTopicArea", b =>
                {
                    b.HasOne("App.Domain.News", "News")
                        .WithMany("HasTopicAreas")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("App.Domain.Project", "Project")
                        .WithMany("HasTopicAreas")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("App.Domain.TopicArea", "TopicArea")
                        .WithMany()
                        .HasForeignKey("TopicAreaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("News");

                    b.Navigation("Project");

                    b.Navigation("TopicArea");
                });

            modelBuilder.Entity("App.Domain.Identity.AppRefreshToken", b =>
                {
                    b.HasOne("App.Domain.Identity.AppUser", "AppUser")
                        .WithMany("AppRefreshTokens")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("App.Domain.Identity.AppRole", b =>
                {
                    b.HasOne("App.Domain.Identity.AppUser", null)
                        .WithMany("Roles")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("App.Domain.TopicArea", b =>
                {
                    b.HasOne("App.Domain.Translations.LanguageString", "LanguageString")
                        .WithOne("TopicArea")
                        .HasForeignKey("App.Domain.TopicArea", "LanguageStringId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("App.Domain.TopicArea", "ParentTopicArea")
                        .WithMany()
                        .HasForeignKey("ParentTopicAreaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("LanguageString");

                    b.Navigation("ParentTopicArea");
                });

            modelBuilder.Entity("App.Domain.Translations.LanguageStringTranslation", b =>
                {
                    b.HasOne("App.Domain.Translations.LanguageString", "LanguageString")
                        .WithMany("LanguageStringTranslations")
                        .HasForeignKey("LanguageStringId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LanguageString");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("App.Domain.Identity.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("App.Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("App.Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("App.Domain.Identity.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("App.Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("App.Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("App.Domain.Identity.AppUser", b =>
                {
                    b.Navigation("AppRefreshTokens");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("App.Domain.News", b =>
                {
                    b.Navigation("Content");

                    b.Navigation("HasTopicAreas");
                });

            modelBuilder.Entity("App.Domain.Project", b =>
                {
                    b.Navigation("Content");

                    b.Navigation("HasTopicAreas");
                });

            modelBuilder.Entity("App.Domain.Translations.LanguageString", b =>
                {
                    b.Navigation("Content");

                    b.Navigation("LanguageStringTranslations");

                    b.Navigation("TopicArea");
                });
#pragma warning restore 612, 618
        }
    }
}
