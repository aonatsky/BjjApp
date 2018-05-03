using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class bracketTimeBarcketTitleAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundTime",
                table: "Bracket",
                nullable: false,
                defaultValue: 600);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Bracket",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundTime",
                table: "Bracket");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Bracket");
        }
    }
}
