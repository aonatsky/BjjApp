using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TRNMNT.Migrations
{
    public partial class InitialMigration : Migration
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
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                schema: "trnmnt",
                columns: table => new
                {
                    TeamID = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamID);
                });

            migrationBuilder.CreateTable(
                name: "WeightDivision",
                schema: "trnmnt",
                columns: table => new
                {
                    WeightDivisionID = table.Column<Guid>(nullable: false),
                    Descritpion = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Weight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightDivision", x => x.WeightDivisionID);
                });

            migrationBuilder.CreateTable(
                name: "Fighter",
                schema: "trnmnt",
                columns: table => new
                {
                    FighterID = table.Column<Guid>(nullable: false),
                    CategoryID = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    TeamID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fighter", x => x.FighterID);
                    table.ForeignKey(
                        name: "FK_Fighter_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "trnmnt",
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fighter_Team_TeamID",
                        column: x => x.TeamID,
                        principalSchema: "trnmnt",
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_CategoryID",
                schema: "trnmnt",
                table: "Fighter",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_TeamID",
                schema: "trnmnt",
                table: "Fighter",
                column: "TeamID");
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
