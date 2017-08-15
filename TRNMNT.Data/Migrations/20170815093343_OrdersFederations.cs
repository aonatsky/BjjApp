using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class OrdersFederations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Federation",
                columns: table => new
                {
                    FederationId = table.Column<Guid>(nullable: false),
                    ContactInformation = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImgPath = table.Column<string>(nullable: true),
                    MembershipPrice = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    UpdateTs = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Federation", x => x.FederationId);
                    table.ForeignKey(
                        name: "FK_Federation_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(nullable: false),
                    Ammount = table.Column<int>(nullable: false),
                    CreateTS = table.Column<DateTime>(nullable: false),
                    OrderType = table.Column<int>(nullable: false),
                    PaymentApproved = table.Column<bool>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    UpdateTS = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FederationMembership",
                columns: table => new
                {
                    FederationMembershipId = table.Column<Guid>(nullable: false),
                    CreateTs = table.Column<DateTime>(nullable: false),
                    FederationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederationMembership", x => x.FederationMembershipId);
                    table.ForeignKey(
                        name: "FK_FederationMembership_Federation_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federation",
                        principalColumn: "FederationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FederationMembership_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Federation_OwnerId",
                table: "Federation",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationMembership_FederationId",
                table: "FederationMembership",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationMembership_UserId",
                table: "FederationMembership",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId1",
                table: "Order",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FederationMembership");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Federation");
        }
    }
}
