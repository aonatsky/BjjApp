using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class OrderIdAddedToMembership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "FederationMembership");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "FederationMembership",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "FederationMembership");

            migrationBuilder.AddColumn<bool>(
                name: "CreateBy",
                table: "FederationMembership",
                nullable: false,
                defaultValue: false);
        }
    }
}
