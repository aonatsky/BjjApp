using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class EventUpdate5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EarlyRegistrationEndTS",
                table: "Event",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EarlyRegistrationPrice",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EarlyRegistrationPriceForMembers",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LateRegistrationPrice",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LateRegistrationPriceForMembers",
                table: "Event",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarlyRegistrationEndTS",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EarlyRegistrationPrice",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EarlyRegistrationPriceForMembers",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LateRegistrationPrice",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LateRegistrationPriceForMembers",
                table: "Event");
        }
    }
}
