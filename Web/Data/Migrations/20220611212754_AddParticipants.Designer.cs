﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web.Data;

#nullable disable

namespace Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220611212754_AddParticipants")]
    partial class AddParticipants
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Web.Data.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("IconId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IconId");

                    b.ToTable("Activities");
                });

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

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MeetingNote")
                        .HasColumnType("text");

                    b.Property<Guid?>("PreviewId")
                        .HasColumnType("uuid");

                    b.Property<string>("Terms")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PreviewId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Web.Data.Entities.EventSpecialization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsRegisteredVolunteersNeeded")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxVolunteersNumber")
                        .HasColumnType("integer");

                    b.Property<int>("MinVolunteersNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Requirements")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventSpecializations");
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

            modelBuilder.Entity("Web.Data.Entities.Participation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EventSpecializationId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMember")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVisited")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("VolunteerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EventSpecializationId");

                    b.HasIndex("VolunteerId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Web.Data.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Review");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Review");
                });

            modelBuilder.Entity("Web.Data.Entities.ReviewScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Positive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ReviewId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReviewId");

                    b.HasIndex("UserId");

                    b.ToTable("ReviewScore");
                });

            modelBuilder.Entity("Web.Data.Entities.Story", b =>
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

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("PreviewId")
                        .HasColumnType("uuid");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PreviewId");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("Web.Data.Entities.StoryActivity", b =>
                {
                    b.Property<Guid>("StoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uuid");

                    b.HasKey("StoryId", "ActivityId");

                    b.HasIndex("ActivityId");

                    b.ToTable("StoryActivity");
                });

            modelBuilder.Entity("Web.Data.Entities.StoryScore", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StoryId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Positive")
                        .HasColumnType("boolean");

                    b.HasKey("UserId", "StoryId");

                    b.HasIndex("StoryId");

                    b.ToTable("StoryScore");
                });

            modelBuilder.Entity("Web.Data.Entities.StoryView", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StoryId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "StoryId");

                    b.HasIndex("StoryId");

                    b.ToTable("StoryView");
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

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

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

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("Web.Data.Entities.UserActivity", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "ActivityId");

                    b.HasIndex("ActivityId");

                    b.ToTable("UserActivity");
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

            modelBuilder.Entity("Web.Data.Entities.Comment", b =>
                {
                    b.HasBaseType("Web.Data.Entities.Review");

                    b.Property<Guid>("StoryId")
                        .HasColumnType("uuid");

                    b.HasIndex("StoryId");

                    b.HasDiscriminator().HasValue("Comment");
                });

            modelBuilder.Entity("Web.Data.Entities.EventReview", b =>
                {
                    b.HasBaseType("Web.Data.Entities.Review");

                    b.Property<int>("CompanyRate")
                        .HasColumnType("integer");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<int>("GoalComplianceRate")
                        .HasColumnType("integer");

                    b.HasIndex("EventId");

                    b.HasDiscriminator().HasValue("EventReview");
                });

            modelBuilder.Entity("Web.Data.Entities.OrganizerUser", b =>
                {
                    b.HasBaseType("Web.Data.Entities.User");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.HasIndex("CompanyId");

                    b.HasDiscriminator().HasValue("OrganizerUser");
                });

            modelBuilder.Entity("Web.Data.Entities.VolunteerUser", b =>
                {
                    b.HasBaseType("Web.Data.Entities.User");

                    b.HasDiscriminator().HasValue("VolunteerUser");
                });

            modelBuilder.Entity("Web.Data.Entities.Activity", b =>
                {
                    b.HasOne("Web.Data.Entities.File", "Icon")
                        .WithMany()
                        .HasForeignKey("IconId");

                    b.Navigation("Icon");
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
                    b.HasOne("Web.Data.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.Company", "Company")
                        .WithMany("Events")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.File", "Preview")
                        .WithMany()
                        .HasForeignKey("PreviewId");

                    b.OwnsMany("Web.Data.Entities.Address", "Locations", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Point>("PointLocation")
                                .HasColumnType("geography (point)");

                            b1.Property<string>("StringLocation")
                                .HasColumnType("text");

                            b1.HasKey("EventId", "Id");

                            b1.ToTable("Events_Locations");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsOne("Web.Data.Entities.DateTimeRange", "Meeting", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("Since")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<DateTime>("Until")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsOne("Web.Data.Entities.DateTimeRange", "Recruitment", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("Since")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<DateTime>("Until")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.Navigation("Activity");

                    b.Navigation("Company");

                    b.Navigation("Locations");

                    b.Navigation("Meeting")
                        .IsRequired();

                    b.Navigation("Preview");

                    b.Navigation("Recruitment");
                });

            modelBuilder.Entity("Web.Data.Entities.EventSpecialization", b =>
                {
                    b.HasOne("Web.Data.Entities.Event", "Event")
                        .WithMany("Specializations")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Web.Data.Entities.Participation", b =>
                {
                    b.HasOne("Web.Data.Entities.EventSpecialization", "EventSpecialization")
                        .WithMany("Participants")
                        .HasForeignKey("EventSpecializationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.VolunteerUser", "Volunteer")
                        .WithMany("Participants")
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventSpecialization");

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("Web.Data.Entities.Review", b =>
                {
                    b.HasOne("Web.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Data.Entities.ReviewScore", b =>
                {
                    b.HasOne("Web.Data.Entities.Review", "Review")
                        .WithMany("ReviewScores")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Review");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Data.Entities.Story", b =>
                {
                    b.HasOne("Web.Data.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.File", "Preview")
                        .WithMany()
                        .HasForeignKey("PreviewId");

                    b.Navigation("Company");

                    b.Navigation("Preview");
                });

            modelBuilder.Entity("Web.Data.Entities.StoryActivity", b =>
                {
                    b.HasOne("Web.Data.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.Story", "Story")
                        .WithMany("StoryActivities")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Story");
                });

            modelBuilder.Entity("Web.Data.Entities.StoryScore", b =>
                {
                    b.HasOne("Web.Data.Entities.Story", "Story")
                        .WithMany("StoryScores")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Story");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Data.Entities.StoryView", b =>
                {
                    b.HasOne("Web.Data.Entities.Story", "Story")
                        .WithMany("StoryViews")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.User", "User")
                        .WithMany("HistoryViews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Story");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Data.Entities.User", b =>
                {
                    b.HasOne("Web.Data.Entities.File", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId");

                    b.OwnsOne("Web.Data.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<Point>("PointLocation")
                                .HasColumnType("geography (point)");

                            b1.Property<string>("StringLocation")
                                .HasColumnType("text");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Address");

                    b.Navigation("Avatar");
                });

            modelBuilder.Entity("Web.Data.Entities.UserActivity", b =>
                {
                    b.HasOne("Web.Data.Entities.Activity", "Activity")
                        .WithMany("UserActivities")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Data.Entities.User", "User")
                        .WithMany("UserActivities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

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

            modelBuilder.Entity("Web.Data.Entities.Comment", b =>
                {
                    b.HasOne("Web.Data.Entities.Story", "Story")
                        .WithMany("Comments")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Story");
                });

            modelBuilder.Entity("Web.Data.Entities.EventReview", b =>
                {
                    b.HasOne("Web.Data.Entities.Event", "Event")
                        .WithMany("EventReviews")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Web.Data.Entities.OrganizerUser", b =>
                {
                    b.HasOne("Web.Data.Entities.Company", "Company")
                        .WithMany("Owners")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Web.Data.Entities.Activity", b =>
                {
                    b.Navigation("UserActivities");
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
                    b.Navigation("EventReviews");

                    b.Navigation("Specializations");
                });

            modelBuilder.Entity("Web.Data.Entities.EventSpecialization", b =>
                {
                    b.Navigation("Participants");
                });

            modelBuilder.Entity("Web.Data.Entities.Review", b =>
                {
                    b.Navigation("ReviewScores");
                });

            modelBuilder.Entity("Web.Data.Entities.Story", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("StoryActivities");

                    b.Navigation("StoryScores");

                    b.Navigation("StoryViews");
                });

            modelBuilder.Entity("Web.Data.Entities.User", b =>
                {
                    b.Navigation("Communications");

                    b.Navigation("HistoryViews");

                    b.Navigation("UserActivities");
                });

            modelBuilder.Entity("Web.Data.Entities.VolunteerUser", b =>
                {
                    b.Navigation("Participants");
                });
#pragma warning restore 612, 618
        }
    }
}
