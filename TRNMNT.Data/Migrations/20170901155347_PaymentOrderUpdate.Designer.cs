using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TRNMNT.Data.Context;

namespace TRNMNT.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170901155347_PaymentOrderUpdate")]
    partial class PaymentOrderUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
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
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("EventId");

                    b.Property<string>("Name");

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

                    b.Property<int>("OrderType");

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

                    b.Property<Guid>("CategoryId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<Guid>("EventId");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("LastName");

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

            modelBuilder.Entity("TRNMNT.Data.Entities.Team", b =>
                {
                    b.Property<Guid>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<Guid>("FederationId");

                    b.Property<string>("Name");

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
                        .HasName("UserNameIndex");

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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.HasOne("TRNMNT.Data.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Category", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Event", "Event")
                        .WithMany("Categories")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Event", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Federation", "Federation")
                        .WithMany()
                        .HasForeignKey("FederationId");

                    b.HasOne("TRNMNT.Data.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Federation", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.FederationMembership", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Federation", "Federation")
                        .WithMany("FederationMemberships")
                        .HasForeignKey("FederationId");

                    b.HasOne("TRNMNT.Data.Entities.User", "User")
                        .WithMany("FederationMemberships")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Fighter", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("TRNMNT.Data.Entities.WeightDivision", "WeightDivision")
                        .WithMany("Fighters")
                        .HasForeignKey("WeightDivisionId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Order", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Participant", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Category", "Category")
                        .WithMany("Participants")
                        .HasForeignKey("CategoryId");

                    b.HasOne("TRNMNT.Data.Entities.Event", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId");

                    b.HasOne("TRNMNT.Data.Entities.Team", "Team")
                        .WithMany("Participants")
                        .HasForeignKey("TeamId");

                    b.HasOne("TRNMNT.Data.Entities.WeightDivision", "WeightDivision")
                        .WithMany("Participants")
                        .HasForeignKey("WeightDivisionId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Team", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Federation", "Federation")
                        .WithMany("Teams")
                        .HasForeignKey("FederationId");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.WeightDivision", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Category", "Category")
                        .WithMany("WeightDivisions")
                        .HasForeignKey("CategoryId");
                });
        }
    }
}
