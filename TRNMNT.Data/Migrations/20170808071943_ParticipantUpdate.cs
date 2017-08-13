using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class ParticipantUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventID",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "EventID",
                table: "Category",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_EventID",
                table: "Category",
                newName: "IX_Category_EventId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Participant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Participant",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTS",
                table: "Participant",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "UpdateTS",
                table: "Participant");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Category",
                newName: "EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Category_EventId",
                table: "Category",
                newName: "IX_Category_EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventID",
                table: "Category",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
