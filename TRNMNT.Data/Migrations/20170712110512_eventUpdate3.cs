using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class eventUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalData",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FBLink",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TNCFilePath",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VKLink",
                table: "Event",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "FBLink",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TNCFilePath",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "VKLink",
                table: "Event");
        }
    }
}
