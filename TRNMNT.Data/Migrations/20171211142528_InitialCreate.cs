using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TRNMNT.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Federation",
                columns: table => new
                {
                    FederationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembershipPrice = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeamRegistrationPrice = table.Column<int>(type: "int", nullable: false),
                    UpdateTs = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Federation", x => x.FederationId);
                    table.ForeignKey(
                        name: "FK_Federation_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreateTS = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderTypeId = table.Column<int>(type: "int", nullable: false),
                    PaymentApproved = table.Column<bool>(type: "bit", nullable: false),
                    PaymentProviderReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateTS = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarlyRegistrationEndTS = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EarlyRegistrationPrice = table.Column<int>(type: "int", nullable: false),
                    EarlyRegistrationPriceForMembers = table.Column<int>(type: "int", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FBLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FederationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LateRegistrationPrice = table.Column<int>(type: "int", nullable: false),
                    LateRegistrationPriceForMembers = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PromoCodeEnabled = table.Column<bool>(type: "bit", nullable: false),
                    PromoCodeListPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationEndTS = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationStartTS = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TNCFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateTS = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrlPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VKLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Event_Federation_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federation",
                        principalColumn: "FederationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FederationMembership",
                columns: table => new
                {
                    FederationMembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateTs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FederationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederationMembership", x => x.FederationMembershipId);
                    table.ForeignKey(
                        name: "FK_FederationMembership_Federation_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federation",
                        principalColumn: "FederationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FederationMembership_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FederationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateTs = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_Federation_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federation",
                        principalColumn: "FederationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoundTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Category_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromoCode",
                columns: table => new
                {
                    PromoCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BurntBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UpdateTs = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCode", x => x.PromoCodeId);
                    table.ForeignKey(
                        name: "FK_PromoCode_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeightDivision",
                columns: table => new
                {
                    WeightDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descritpion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightDivision", x => x.WeightDivisionId);
                    table.ForeignKey(
                        name: "FK_WeightDivision_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fighter",
                columns: table => new
                {
                    FighterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateTs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeightDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fighter", x => x.FighterId);
                    table.ForeignKey(
                        name: "FK_Fighter_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fighter_WeightDivision_WeightDivisionId",
                        column: x => x.WeightDivisionId,
                        principalTable: "WeightDivision",
                        principalColumn: "WeightDivisionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivatedPromoCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateTS = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeightDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.ParticipantId);
                    table.ForeignKey(
                        name: "FK_Participant_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participant_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participant_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participant_WeightDivision_WeightDivisionId",
                        column: x => x.WeightDivisionId,
                        principalTable: "WeightDivision",
                        principalColumn: "WeightDivisionId",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateTable(
                name: "Bracket",
                columns: table => new
                {
                    BracketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeightDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bracket", x => x.BracketId);
                    table.ForeignKey(
                        name: "FK_Bracket_WeightDivision_WeightDivisionId",
                        column: x => x.WeightDivisionId,
                        principalTable: "WeightDivision",
                        principalColumn: "WeightDivisionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BracketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NextRoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.RoundId);
                    table.ForeignKey(
                        name: "FK_Round_Bracket_BracketId",
                        column: x => x.BracketId,
                        principalTable: "Bracket",
                        principalColumn: "BracketId",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Round_Participant_FirstParticipantId",
                        column: x => x.FirstParticipantId,
                        principalTable: "Participant",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Round_Participant_SecondParticipantId",
                        column: x => x.SecondParticipantId,
                        principalTable: "Participant",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Round_Participant_WinnerParticipantId",
                        column: x => x.WinnerParticipantId,
                        principalTable: "Participant",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Round_Round_NextRoundId",
                        column: x => x.NextRoundId,
                        principalTable: "Round",
                        principalColumn: "RoundId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Category_EventId",
                table: "Category",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_FederationId",
                table: "Event",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_OwnerId",
                table: "Event",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Federation_OwnerId",
                table: "Federation",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationMembership_FederationId",
                table: "FederationMembership",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationMembership_UserId",
                table: "FederationMembership",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_TeamId",
                table: "Fighter",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_WeightDivisionId",
                table: "Fighter",
                column: "WeightDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_CategoryId",
                table: "Participant",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_EventId",
                table: "Participant",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_TeamId",
                table: "Participant",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_WeightDivisionId",
                table: "Participant",
                column: "WeightDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCode_EventId",
                table: "PromoCode",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_FederationId",
                table: "Team",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightDivision_CategoryId",
                table: "WeightDivision",
                column: "CategoryId");

            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON ");
            migrationBuilder.Sql("INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [DateOfBirth]) VALUES (N'a2070395-168a-49aa-972b-326edd47bf70', 0, N'809b75de-ce7b-4a66-b4cc-ff8ae303c3ad', N'Ivan.drago@trnmnt.com', 0, N'Ivan', N'Drago', 1, NULL, N'IVAN.DRAGO@TRNMNT.COM', N'ADMIN', N'AQAAAAEAACcQAAAAEOGcrW0OF0EzOc38l11oNC1y8SfFoMQOeqxgXnA44Jh5+L3fds3KgTzGADvUBc05uw==', NULL, 0, N'ca0d2931-6c19-4c68-b855-13dad950e6f4', 0, N'admin', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))");
            migrationBuilder.Sql("INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (3, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Owner', N'a2070395-168a-49aa-972b-326edd47bf70')");
            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF ");
            migrationBuilder.Sql("INSERT [dbo].[Federation] ([FederationId], [ContactInformation], [Currency], [Description], [ImgPath], [MembershipPrice], [Name], [OwnerId], [UpdateTs], [TeamRegistrationPrice]) VALUES (N'673ea3ce-2530-48c0-b84c-a3de492cab25', NULL, N'UAH', N'Ukrainian Federation Of Brazilian Jiu Jitsu', NULL, 5, N'UABJJF', N'a2070395-168a-49aa-972b-326edd47bf70', CAST(N'2017-08-15T17:25:38.2630000' AS DateTime2), 0)");
            migrationBuilder.Sql("INSERT [dbo].[Event] ([EventId], [Address], [Description], [EventDate], [ImgPath], [IsActive], [OwnerId], [RegistrationEndTS], [RegistrationStartTS], [StatusId], [Title], [UpdateTS], [UrlPrefix], [AdditionalData], [CardNumber], [ContactEmail], [ContactPhone], [FBLink], [TNCFilePath], [VKLink], [EarlyRegistrationEndTS], [EarlyRegistrationPrice], [EarlyRegistrationPriceForMembers], [LateRegistrationPrice], [LateRegistrationPriceForMembers], [FederationId], [PromoCodeEnabled], [PromoCodeListPath]) VALUES (N'60e9da46-c660-4a9b-5c43-08d4c85dc994', N'Conroe High School Pit Gym - 3200 W Davis St, Conroe Tx 77304', N'$85 for 1 division, $95 for 2 ($10 off during early online registration period). Early weigh ins are AT THE VENUE the night before at 5:30-7:30 pm and the morning of the event between 8-9:30 am. Please be assured that we make all possible effort to match all competitors (including those who fall in the women''s 150+ division) as closely as possible according to age, weight and rank. Should the event reach capacity, we reserve the right to close online registration early and to restrict registration at the door as we see fit.', CAST(N'2017-10-04T09:00:00.0000000' AS DateTime2), '', 1, N'a2070395-168a-49aa-972b-326edd47bf70', CAST(N'2017-05-29T06:00:00.0000000' AS DateTime2), CAST(N'2017-05-01T06:00:00.0000000' AS DateTime2), 1, N'Kiev open 2022', CAST(N'2017-09-28T15:42:46.2163470' AS DateTime2), N'kievopen2017', N'In-person registration is available for this event. If you are unable to register online, you may register in-person at the venue during early weigh-ins or the morning of the event. In-person registrations are not eligible for any promotional items or discounts that are available online.', NULL, N'bjjukraine@gmail.com', N'+380501234567', N'https://www.facebook.com/events/1218851341576411', '', N'https://vk.com/bjjfreaks', CAST(N'2017-06-01T06:00:00.0000000' AS DateTime2), 95, 8, 8, 13, N'673ea3ce-2530-48c0-b84c-a3de492cab25', 1, NULL)");
            migrationBuilder.Sql("INSERT [dbo].[Category] ([CategoryId], [Name], [EventId], [RoundTime]) VALUES (N'8edc9c5c-5bdd-4376-8415-26a886b738b0', N'KIDS 1', N'60e9da46-c660-4a9b-5c43-08d4c85dc994',300)");
            migrationBuilder.Sql("INSERT [dbo].[Category] ([CategoryId], [Name], [EventId], [RoundTime]) VALUES (N'7198b661-e612-40f5-af17-839526393d07', N'Category TEST', N'60e9da46-c660-4a9b-5c43-08d4c85dc994',300)");
            migrationBuilder.Sql("INSERT [dbo].[Category] ([CategoryId], [Name], [EventId], [RoundTime]) VALUES (N'02b44457-be9e-4196-a666-e5602e04ee61', N'Male Balck Belt', N'60e9da46-c660-4a9b-5c43-08d4c85dc994',300)");
            migrationBuilder.Sql("INSERT [dbo].[Category] ([CategoryId], [Name], [EventId], [RoundTime]) VALUES (N'02b44457-be9e-4196-a666-e5602e04ee63', N'Female Black', N'60e9da46-c660-4a9b-5c43-08d4c85dc994',300)");
            migrationBuilder.Sql("INSERT [dbo].[WeightDivision] ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId]) VALUES (N'83ca0f48-1bf7-4441-e54a-08d4c85dc99e', NULL, N'-90 Heavy', 0, N'02b44457-be9e-4196-a666-e5602e04ee61')");
            migrationBuilder.Sql("INSERT [dbo].[WeightDivision] ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId]) VALUES (N'0e397f40-c6f9-40f0-e54b-08d4c85dc99e', NULL, N'-70 Light', 0, N'02b44457-be9e-4196-a666-e5602e04ee61')");
            migrationBuilder.Sql("INSERT [dbo].[WeightDivision] ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId]) VALUES (N'5b454a2f-8bd1-4a83-e54d-08d4c85dc99e', NULL, N'- 65 Light', 0, N'02b44457-be9e-4196-a666-e5602e04ee63')");
            migrationBuilder.Sql("INSERT [dbo].[WeightDivision] ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId]) VALUES (N'84dbfbc7-a754-426c-e54e-08d4c85dc99e', NULL, N'+65 Heavy', 0, N'02b44457-be9e-4196-a666-e5602e04ee63')");
            migrationBuilder.Sql("INSERT [dbo].[WeightDivision] ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId]) VALUES (N'a40b083e-cf0e-4445-b26b-36f462afe104', NULL, N'Weight Divison 44', 0, N'7198b661-e612-40f5-af17-839526393d07')");
            migrationBuilder.Sql("INSERT [dbo].[WeightDivision] ([WeightDivisionId], [Descritpion], [Name], [Weight], [CategoryId]) VALUES (N'0f2641d3-64e4-4aab-ba65-8ee4306a8e5e', NULL, N'Weight Division', 0, N'8edc9c5c-5bdd-4376-8415-26a886b738b0')");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FederationMembership");

            migrationBuilder.DropTable(
                name: "Fighter");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "PromoCode");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "WeightDivision");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Federation");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
