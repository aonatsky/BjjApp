using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class EventStatusAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "WeightDivision",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Fighter",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Fighter",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTs",
                table: "Fighter",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_WeightDivision_CategoryId",
                table: "WeightDivision",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightDivision_Category_CategoryId",
                table: "WeightDivision",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightDivision_Category_CategoryId",
                table: "WeightDivision");

            migrationBuilder.DropIndex(
                name: "IX_WeightDivision_CategoryId",
                table: "WeightDivision");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "WeightDivision");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Fighter");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Fighter");

            migrationBuilder.DropColumn(
                name: "UpdateTs",
                table: "Fighter");
        }
    }
}
