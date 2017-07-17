using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class eventUpdate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Category_CategoryId",
                table: "Fighter");

            migrationBuilder.DropIndex(
                name: "IX_Fighter_CategoryId",
                table: "Fighter");

            migrationBuilder.RenameColumn(
                name: "Descritpion",
                table: "Event",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Category",
                newName: "EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Category_EventId",
                table: "Category",
                newName: "IX_Category_EventID");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventID",
                table: "Category",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventID",
                table: "Category",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventID",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Event",
                newName: "Descritpion");

            migrationBuilder.RenameColumn(
                name: "EventID",
                table: "Category",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_EventID",
                table: "Category",
                newName: "IX_Category_EventId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Category",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_CategoryId",
                table: "Fighter",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Category_CategoryId",
                table: "Fighter",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
