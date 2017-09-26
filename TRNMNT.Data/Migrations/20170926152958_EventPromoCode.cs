using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class EventPromoCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PromoCodeEnabled",
                table: "Event",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PromoCodeListPath",
                table: "Event",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromoCodeEnabled",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "PromoCodeListPath",
                table: "Event");
        }
    }
}
