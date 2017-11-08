using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class paymentUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderType",
                table: "Order",
                newName: "OrderTypeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Team",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Team",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Participant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Participant");

            migrationBuilder.RenameColumn(
                name: "OrderTypeId",
                table: "Order",
                newName: "OrderType");
        }
    }
}
