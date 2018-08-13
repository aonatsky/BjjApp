using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class TeamWithApprovalAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "Team",
                newName: "OwnerId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Federation",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommissionPercentage",
                table: "Federation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinCommission",
                table: "Federation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamMembershipApprovalStatus",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeamId",
                table: "AspNetUsers",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Team_TeamId",
                table: "AspNetUsers",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Team_TeamId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CommissionPercentage",
                table: "Federation");

            migrationBuilder.DropColumn(
                name: "MinCommission",
                table: "Federation");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeamMembershipApprovalStatus",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Team",
                newName: "CreateBy");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Federation",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
