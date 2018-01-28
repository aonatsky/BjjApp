﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TRNMNT.Data.Context;

namespace TRNMNT.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

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
                        .ValueGeneratedOnAdd();

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
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

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

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Bracket", b =>
                {
                    b.Property<Guid>("BracketId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("WeightDivisionId");

                    b.HasKey("BracketId");

                    b.HasIndex("WeightDivisionId");

                    b.ToTable("Bracket");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("EventId");

                    b.Property<string>("Name");

                    b.Property<int>("RoundTime");

                    b.HasKey("CategoryId");

                    b.HasIndex("EventId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Event", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalData");

                    b.Property<string>("Address");

                    b.Property<string>("CardNumber");

                    b.Property<string>("ContactEmail");

                    b.Property<string>("ContactPhone");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EarlyRegistrationEndTS");

                    b.Property<int>("EarlyRegistrationPrice");

                    b.Property<int>("EarlyRegistrationPriceForMembers");

                    b.Property<DateTime>("EventDate");

                    b.Property<string>("FBLink");

                    b.Property<Guid>("FederationId");

                    b.Property<string>("ImgPath");

                    b.Property<bool>("IsActive");

                    b.Property<int>("LateRegistrationPrice");

                    b.Property<int>("LateRegistrationPriceForMembers");

                    b.Property<string>("OwnerId");

                    b.Property<bool>("PromoCodeEnabled");

                    b.Property<string>("PromoCodeListPath");

                    b.Property<DateTime>("RegistrationEndTS");

                    b.Property<DateTime>("RegistrationStartTS");

                    b.Property<int>("StatusId");

                    b.Property<string>("TNCFilePath");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdateTS");

                    b.Property<string>("UrlPrefix");

                    b.Property<string>("VKLink");

                    b.HasKey("EventId");

                    b.HasIndex("FederationId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Federation", b =>
                {
                    b.Property<Guid>("FederationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactInformation");

                    b.Property<string>("Currency");

                    b.Property<string>("Description");

                    b.Property<string>("ImgPath");

                    b.Property<int>("MembershipPrice");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<int>("TeamRegistrationPrice");

                    b.Property<DateTime>("UpdateTs");

                    b.HasKey("FederationId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Federation");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.FederationMembership", b =>
                {
                    b.Property<Guid>("FederationMembershipId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTs");

                    b.Property<Guid>("FederationId");

                    b.Property<string>("UserId");

                    b.HasKey("FederationMembershipId");

                    b.HasIndex("FederationId");

                    b.HasIndex("UserId");

                    b.ToTable("FederationMembership");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Fighter", b =>
                {
                    b.Property<Guid>("FighterId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("LastName");

                    b.Property<Guid>("TeamId");

                    b.Property<DateTime>("UpdateTs");

                    b.Property<Guid>("WeightDivisionId");

                    b.HasKey("FighterId");

                    b.HasIndex("TeamId");

                    b.HasIndex("WeightDivisionId");

                    b.ToTable("Fighter");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<DateTime>("CreateTS");

                    b.Property<string>("Currency");

                    b.Property<int>("OrderTypeId");

                    b.Property<bool>("PaymentApproved");

                    b.Property<string>("PaymentProviderReference");

                    b.Property<string>("Reference");

                    b.Property<DateTime>("UpdateTS");

                    b.Property<string>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Participant", b =>
                {
                    b.Property<Guid>("ParticipantId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivatedPromoCode");

                    b.Property<Guid>("CategoryId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<Guid>("EventId");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("LastName");

                    b.Property<Guid?>("OrderId");

                    b.Property<string>("PhoneNumber");

                    b.Property<Guid>("TeamId");

                    b.Property<DateTime>("UpdateTS");

                    b.Property<string>("UserId");

                    b.Property<Guid>("WeightDivisionId");

                    b.HasKey("ParticipantId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("EventId");

                    b.HasIndex("TeamId");

                    b.HasIndex("WeightDivisionId");

                    b.ToTable("Participant");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.PromoCode", b =>
                {
                    b.Property<Guid>("PromoCodeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BurntBy");

                    b.Property<string>("Code");

                    b.Property<Guid>("EventId");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("UpdateTs");

                    b.HasKey("PromoCodeId");

                    b.HasIndex("EventId");

                    b.ToTable("PromoCode");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Round", b =>
                {
                    b.Property<Guid>("RoundId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BracketId");

                    b.Property<Guid?>("FirstParticipantId");

                    b.Property<bool>("HasBooferParticipant");

                    b.Property<Guid?>("NextRoundId");

                    b.Property<Guid?>("SecondParticipantId");

                    b.Property<int>("Stage");

                    b.Property<Guid?>("WinnerParticipantId");

                    b.HasKey("RoundId");

                    b.HasIndex("BracketId");

                    b.HasIndex("FirstParticipantId");

                    b.HasIndex("NextRoundId");

                    b.HasIndex("SecondParticipantId");

                    b.HasIndex("WinnerParticipantId");

                    b.ToTable("Round");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Team", b =>
                {
                    b.Property<Guid>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<Guid>("FederationId");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("Name");

                    b.Property<Guid?>("OrderId");

                    b.Property<string>("UpdateBy");

                    b.Property<DateTime>("UpdateTs");

                    b.HasKey("TeamId");

                    b.HasIndex("FederationId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

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

            modelBuilder.Entity("TRNMNT.Data.Entities.WeightDivision", b =>
                {
                    b.Property<Guid>("WeightDivisionId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<string>("Descritpion");

                    b.Property<string>("Name");

                    b.Property<int>("Weight");

                    b.HasKey("WeightDivisionId");

                    b.HasIndex("CategoryId");

                    b.ToTable("WeightDivision");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Bracket", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.WeightDivision", "WeightDivision")
                        .WithMany("Brackets")
                        .HasForeignKey("WeightDivisionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Category", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Event", "Event")
                        .WithMany("Categories")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Event", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Federation", "Federation")
                        .WithMany()
                        .HasForeignKey("FederationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Federation", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.FederationMembership", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Federation", "Federation")
                        .WithMany("FederationMemberships")
                        .HasForeignKey("FederationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.User", "User")
                        .WithMany("FederationMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Fighter", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.WeightDivision", "WeightDivision")
                        .WithMany("Fighters")
                        .HasForeignKey("WeightDivisionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Order", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Participant", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Category", "Category")
                        .WithMany("Participants")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.Event", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.Team", "Team")
                        .WithMany("Participants")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.WeightDivision", "WeightDivision")
                        .WithMany("Participants")
                        .HasForeignKey("WeightDivisionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.PromoCode", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Event", "Event")
                        .WithMany("PromoCodes")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Round", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Bracket", "Bracket")
                        .WithMany("Rounds")
                        .HasForeignKey("BracketId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.Participant", "FirstParticipant")
                        .WithMany()
                        .HasForeignKey("FirstParticipantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.Round", "NextRound")
                        .WithMany()
                        .HasForeignKey("NextRoundId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.Participant", "SecondParticipant")
                        .WithMany()
                        .HasForeignKey("SecondParticipantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TRNMNT.Data.Entities.Participant", "WinnerParticipant")
                        .WithMany()
                        .HasForeignKey("WinnerParticipantId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Team", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Federation", "Federation")
                        .WithMany("Teams")
                        .HasForeignKey("FederationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.WeightDivision", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Category", "Category")
                        .WithMany("WeightDivisions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
