using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class FederationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FederationId",
                table: "Team",
                nullable: false,
                defaultValue: new Guid("673EA3CE-2530-48C0-B84C-A3DE492CAB25"));

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Team",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTs",
                table: "Team",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TeamRegistrationPrice",
                table: "Federation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Team_FederationId",
                table: "Team",
                column: "FederationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Federation_FederationId",
                table: "Team",
                column: "FederationId",
                principalTable: "Federation",
                principalColumn: "FederationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Federation_FederationId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_FederationId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "FederationId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "UpdateTs",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "TeamRegistrationPrice",
                table: "Federation");
        }
    }
}
