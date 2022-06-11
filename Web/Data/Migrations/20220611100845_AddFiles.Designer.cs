﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web.Data;

#nullable disable

namespace Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220611100845_AddFiles")]
    partial class AddFiles
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Web.Data.Entities.Communication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Communications");
                });

            modelBuilder.Entity("Web.Data.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Web.Data.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRegisteredVolunteersNeeded")
                        .HasColumnType("boolean");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MaxVolunteersNumber")
                        .HasColumnType("integer");

                    b.Property<int>("MinVolunteersNumber")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Since")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Until")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Web.Data.Entities.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Web.Data.Entities.Interest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Interests");
                });

            modelBuilder.Entity("Web.Data.Entities.Participation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsMember")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("VolunteerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("VolunteerId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Web.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<Guid?>("AvatarId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AvatarId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Web.Data.Entities.UserInterest", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("InterestId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "InterestId");

                    b.HasIndex("InterestId");

                    b.ToTable("UserInterest");
                });

            modelBuilder.Entity("Web.Data.Entities.Verification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CommunicationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CommunicationId");

                    b.ToTable("CommunicationVerifications");
                });

            modelBuilder.Entity("Web.Data.Entities.Communication", b =>
                {
                    b.HasOne("Web.Data.Entities.User", "User")
                        .WithMany("Communications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Data.Entities.Event", b =>
                {
                    b.HasOne("Web.Data.Entities.Company", "Company")
                        .WithMany("Events")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Web.Data.Entities.Participation", b =>
                {
                    b.HasOne("Web.Data.Entities.Event", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.User", "Volunteer")
                        .WithMany("Participants")
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("Web.Data.Entities.User", b =>
                {
                    b.HasOne("Web.Data.Entities.File", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId");

                    b.HasOne("Web.Data.Entities.Company", null)
                        .WithMany("Owners")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Avatar");
                });

            modelBuilder.Entity("Web.Data.Entities.UserInterest", b =>
                {
                    b.HasOne("Web.Data.Entities.Interest", "Interest")
                        .WithMany("UserInterests")
                        .HasForeignKey("InterestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.User", "User")
                        .WithMany("UserInterests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Interest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Data.Entities.Verification", b =>
                {
                    b.HasOne("Web.Data.Entities.Communication", "Communication")
                        .WithMany("Verifications")
                        .HasForeignKey("CommunicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Communication");
                });

            modelBuilder.Entity("Web.Data.Entities.Communication", b =>
                {
                    b.Navigation("Verifications");
                });

            modelBuilder.Entity("Web.Data.Entities.Company", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Owners");
                });

            modelBuilder.Entity("Web.Data.Entities.Event", b =>
                {
                    b.Navigation("Participants");
                });

            modelBuilder.Entity("Web.Data.Entities.Interest", b =>
                {
                    b.Navigation("UserInterests");
                });

            modelBuilder.Entity("Web.Data.Entities.User", b =>
                {
                    b.Navigation("Communications");

                    b.Navigation("Participants");

                    b.Navigation("UserInterests");
                });
#pragma warning restore 612, 618
        }
    }
}
