using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class RoundUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoundResultDetails",
                table: "Round",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoundResultType",
                table: "Round",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundResultDetails",
                table: "Round");

            migrationBuilder.DropColumn(
                name: "RoundResultType",
                table: "Round");
        }
    }
}
