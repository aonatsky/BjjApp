using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class CategoryBracketCompleteTsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompleteTs",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompleteTs",
                table: "Bracket",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTs",
                table: "Bracket",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteTs",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CompleteTs",
                table: "Bracket");

            migrationBuilder.DropColumn(
                name: "StartTs",
                table: "Bracket");
        }
    }
}
