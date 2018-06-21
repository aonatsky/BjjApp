using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class matchrefactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Round_Bracket_BracketId",
                table: "Round");

            migrationBuilder.DropForeignKey(
                name: "FK_Round_Round_NextRoundId",
                table: "Round");

            migrationBuilder.DropTable(
                name: "Bracket");

            migrationBuilder.DropTable(
                name: "Fighter");

            migrationBuilder.RenameColumn(
                name: "Stage",
                table: "Round",
                newName: "Round");

            migrationBuilder.RenameColumn(
                name: "RoundType",
                table: "Round",
                newName: "MatchType");

            migrationBuilder.RenameColumn(
                name: "RoundResultType",
                table: "Round",
                newName: "MatchResultType");

            migrationBuilder.RenameColumn(
                name: "RoundResultDetails",
                table: "Round",
                newName: "MatchResultDetails");

            migrationBuilder.RenameColumn(
                name: "NextRoundId",
                table: "Round",
                newName: "NextMatchId");

            migrationBuilder.RenameColumn(
                name: "BracketId",
                table: "Round",
                newName: "WeightDivisionId");

            migrationBuilder.RenameColumn(
                name: "RoundId",
                table: "Round",
                newName: "MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Round_NextRoundId",
                table: "Round",
                newName: "IX_Round_NextMatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Round_BracketId",
                table: "Round",
                newName: "IX_Round_WeightDivisionId");

            migrationBuilder.RenameColumn(
                name: "RoundTime",
                table: "Category",
                newName: "MatchTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompleteTs",
                table: "WeightDivision",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTs",
                table: "WeightDivision",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Round",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDisqualified",
                table: "Participant",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Round_CategoryId",
                table: "Round",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Round_Category_CategoryId",
                table: "Round",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Round_Round_NextMatchId",
                table: "Round",
                column: "NextMatchId",
                principalTable: "Round",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Round_WeightDivision_WeightDivisionId",
                table: "Round",
                column: "WeightDivisionId",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Round_Category_CategoryId",
                table: "Round");

            migrationBuilder.DropForeignKey(
                name: "FK_Round_Round_NextMatchId",
                table: "Round");

            migrationBuilder.DropForeignKey(
                name: "FK_Round_WeightDivision_WeightDivisionId",
                table: "Round");

            migrationBuilder.DropIndex(
                name: "IX_Round_CategoryId",
                table: "Round");

            migrationBuilder.DropColumn(
                name: "CompleteTs",
                table: "WeightDivision");

            migrationBuilder.DropColumn(
                name: "StartTs",
                table: "WeightDivision");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Round");

            migrationBuilder.DropColumn(
                name: "IsDisqualified",
                table: "Participant");

            migrationBuilder.RenameColumn(
                name: "WeightDivisionId",
                table: "Round",
                newName: "BracketId");

            migrationBuilder.RenameColumn(
                name: "Round",
                table: "Round",
                newName: "Stage");

            migrationBuilder.RenameColumn(
                name: "NextMatchId",
                table: "Round",
                newName: "NextRoundId");

            migrationBuilder.RenameColumn(
                name: "MatchType",
                table: "Round",
                newName: "RoundType");

            migrationBuilder.RenameColumn(
                name: "MatchResultType",
                table: "Round",
                newName: "RoundResultType");

            migrationBuilder.RenameColumn(
                name: "MatchResultDetails",
                table: "Round",
                newName: "RoundResultDetails");

            migrationBuilder.RenameColumn(
                name: "MatchId",
                table: "Round",
                newName: "RoundId");

            migrationBuilder.RenameIndex(
                name: "IX_Round_WeightDivisionId",
                table: "Round",
                newName: "IX_Round_BracketId");

            migrationBuilder.RenameIndex(
                name: "IX_Round_NextMatchId",
                table: "Round",
                newName: "IX_Round_NextRoundId");

            migrationBuilder.RenameColumn(
                name: "MatchTime",
                table: "Category",
                newName: "RoundTime");

            migrationBuilder.CreateTable(
                name: "Bracket",
                columns: table => new
                {
                    BracketId = table.Column<Guid>(nullable: false),
                    CompleteTs = table.Column<DateTime>(nullable: true),
                    RoundTime = table.Column<int>(nullable: false),
                    StartTs = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    WeightDivisionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bracket", x => x.BracketId);
                    table.ForeignKey(
                        name: "FK_Bracket_WeightDivision_WeightDivisionId",
                        column: x => x.WeightDivisionId,
                        principalTable: "WeightDivision",
                        principalColumn: "WeightDivisionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fighter",
                columns: table => new
                {
                    FighterId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    TeamId = table.Column<Guid>(nullable: false),
                    UpdateTs = table.Column<DateTime>(nullable: false),
                    WeightDivisionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fighter", x => x.FighterId);
                    table.ForeignKey(
                        name: "FK_Fighter_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fighter_WeightDivision_WeightDivisionId",
                        column: x => x.WeightDivisionId,
                        principalTable: "WeightDivision",
                        principalColumn: "WeightDivisionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bracket_WeightDivisionId",
                table: "Bracket",
                column: "WeightDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_TeamId",
                table: "Fighter",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_WeightDivisionId",
                table: "Fighter",
                column: "WeightDivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Round_Bracket_BracketId",
                table: "Round",
                column: "BracketId",
                principalTable: "Bracket",
                principalColumn: "BracketId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Round_Round_NextRoundId",
                table: "Round",
                column: "NextRoundId",
                principalTable: "Round",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
