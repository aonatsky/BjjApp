using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class EventUpdate6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FederationId",
                table: "Event",
                nullable: false,
                defaultValue: new Guid("673ea3ce-2530-48c0-b84c-a3de492cab25"));

            migrationBuilder.CreateIndex(
                name: "IX_Event_FederationId",
                table: "Event",
                column: "FederationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Federation_FederationId",
                table: "Event",
                column: "FederationId",
                principalTable: "Federation",
                principalColumn: "FederationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Federation_FederationId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_FederationId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "FederationId",
                table: "Event");
        }
    }
}
