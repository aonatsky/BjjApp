using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "trnmnt");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "trnmnt",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                schema: "trnmnt",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "WeightDivision",
                schema: "trnmnt",
                columns: table => new
                {
                    WeightDivisionId = table.Column<Guid>(nullable: false),
                    Descritpion = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Weight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightDivision", x => x.WeightDivisionId);
                });

            migrationBuilder.CreateTable(
                name: "Fighter",
                schema: "trnmnt",
                columns: table => new
                {
                    FighterId = table.Column<Guid>(nullable: false),
                    CategoryID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fighter", x => x.FighterId);
                    table.ForeignKey(
                        name: "FK_Fighter_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "trnmnt",
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fighter_Team_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "trnmnt",
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_CategoryID",
                schema: "trnmnt",
                table: "Fighter",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_TeamId",
                schema: "trnmnt",
                table: "Fighter",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fighter",
                schema: "trnmnt");

            migrationBuilder.DropTable(
                name: "WeightDivision",
                schema: "trnmnt");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "trnmnt");

            migrationBuilder.DropTable(
                name: "Team",
                schema: "trnmnt");
        }
    }
}
