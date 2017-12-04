using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TRNMNT.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Federation_FederationId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_AspNetUsers_OwnerId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Federation_AspNetUsers_OwnerId",
                table: "Federation");

            migrationBuilder.DropForeignKey(
                name: "FK_FederationMembership_Federation_FederationId",
                table: "FederationMembership");

            migrationBuilder.DropForeignKey(
                name: "FK_FederationMembership_AspNetUsers_UserId",
                table: "FederationMembership");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Team_TeamId",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Category_CategoryId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Event_EventId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Team_TeamId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_WeightDivision_WeightDivisionId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCode_Event_EventId",
                table: "PromoCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Federation_FederationId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightDivision_Category_CategoryId",
                table: "WeightDivision");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Federation_FederationId",
                table: "Event",
                column: "FederationId",
                principalTable: "Federation",
                principalColumn: "FederationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_AspNetUsers_OwnerId",
                table: "Event",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Federation_AspNetUsers_OwnerId",
                table: "Federation",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FederationMembership_Federation_FederationId",
                table: "FederationMembership",
                column: "FederationId",
                principalTable: "Federation",
                principalColumn: "FederationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FederationMembership_AspNetUsers_UserId",
                table: "FederationMembership",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Team_TeamId",
                table: "Fighter",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                table: "Fighter",
                column: "WeightDivisionId",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Category_CategoryId",
                table: "Participant",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Event_EventId",
                table: "Participant",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Team_TeamId",
                table: "Participant",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_WeightDivision_WeightDivisionId",
                table: "Participant",
                column: "WeightDivisionId",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCode_Event_EventId",
                table: "PromoCode",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Federation_FederationId",
                table: "Team",
                column: "FederationId",
                principalTable: "Federation",
                principalColumn: "FederationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightDivision_Category_CategoryId",
                table: "WeightDivision",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("INSERT [dbo].[Federation] ([FederationId], [ContactInformation], [Currency], [Description], [ImgPath], [MembershipPrice], [Name], [OwnerId], [UpdateTs], [TeamRegistrationPrice]) VALUES (N'673ea3ce-2530-48c0-b84c-a3de492cab25', NULL, N'UAH', N'Ukrainian Federation Of Brazilian Jiu Jitsu', NULL, 5, N'UABJJF', N'a2070395-168a-49aa-972b-326edd47bf70', CAST(N'2017-08-15 17:25:38.2630000' AS DateTime2), 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Federation_FederationId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_AspNetUsers_OwnerId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Federation_AspNetUsers_OwnerId",
                table: "Federation");

            migrationBuilder.DropForeignKey(
                name: "FK_FederationMembership_Federation_FederationId",
                table: "FederationMembership");

            migrationBuilder.DropForeignKey(
                name: "FK_FederationMembership_AspNetUsers_UserId",
                table: "FederationMembership");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Team_TeamId",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Category_CategoryId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Event_EventId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Team_TeamId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_WeightDivision_WeightDivisionId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCode_Event_EventId",
                table: "PromoCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Federation_FederationId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightDivision_Category_CategoryId",
                table: "WeightDivision");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Federation_FederationId",
                table: "Event",
                column: "FederationId",
                principalTable: "Federation",
                principalColumn: "FederationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_AspNetUsers_OwnerId",
                table: "Event",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Federation_AspNetUsers_OwnerId",
                table: "Federation",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FederationMembership_Federation_FederationId",
                table: "FederationMembership",
                column: "FederationId",
                principalTable: "Federation",
                principalColumn: "FederationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FederationMembership_AspNetUsers_UserId",
                table: "FederationMembership",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Team_TeamId",
                table: "Fighter",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                table: "Fighter",
                column: "WeightDivisionId",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Category_CategoryId",
                table: "Participant",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Event_EventId",
                table: "Participant",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Team_TeamId",
                table: "Participant",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_WeightDivision_WeightDivisionId",
                table: "Participant",
                column: "WeightDivisionId",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCode_Event_EventId",
                table: "PromoCode",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Federation_FederationId",
                table: "Team",
                column: "FederationId",
                principalTable: "Federation",
                principalColumn: "FederationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightDivision_Category_CategoryId",
                table: "WeightDivision",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
