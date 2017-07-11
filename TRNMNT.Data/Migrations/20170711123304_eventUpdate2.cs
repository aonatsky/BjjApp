using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class eventUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Category",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Descritpion = table.Column<string>(nullable: true),
                    EventDate = table.Column<DateTime>(nullable: false),
                    ImgPath = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    RegistrationEndTS = table.Column<DateTime>(nullable: false),
                    RegistrationStartTS = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UpdateTS = table.Column<DateTime>(nullable: false),
                    UrlPrefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Event_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_EventId",
                table: "Category",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_OwnerId",
                table: "Event",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventId",
                table: "Category");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Category_EventId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Category");
        }
    }
}
