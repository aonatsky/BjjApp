using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TRNMNT.Data.Migrations
{
    public partial class RoundTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBooferParticipant",
                table: "Round");

            migrationBuilder.AddColumn<int>(
                name: "RoundType",
                table: "Round",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundType",
                table: "Round");

            migrationBuilder.AddColumn<bool>(
                name: "HasBooferParticipant",
                table: "Round",
                nullable: false,
                defaultValue: false);
        }
    }
}
