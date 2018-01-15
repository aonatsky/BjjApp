using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TRNMNT.Data.Migrations
{
    public partial class RoundBracketAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bracket",
                columns: table => new
                {
                    BracketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeightDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "Round",
                columns: table => new
                {
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BracketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextRoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecondParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinnerParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.RoundId);
                    table.ForeignKey(
                        name: "FK_Round_Bracket_BracketId",
                        column: x => x.BracketId,
                        principalTable: "Bracket",
                        principalColumn: "BracketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Round_Participant_FirstParticipantId",
                        column: x => x.FirstParticipantId,
                        principalTable: "Participant",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Round_Round_NextRoundId",
                        column: x => x.NextRoundId,
                        principalTable: "Round",
                        principalColumn: "RoundId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Round_Participant_SecondParticipantId",
                        column: x => x.SecondParticipantId,
                        principalTable: "Participant",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Round_Participant_WinnerParticipantId",
                        column: x => x.WinnerParticipantId,
                        principalTable: "Participant",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bracket_WeightDivisionId",
                table: "Bracket",
                column: "WeightDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_BracketId",
                table: "Round",
                column: "BracketId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_FirstParticipantId",
                table: "Round",
                column: "FirstParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_NextRoundId",
                table: "Round",
                column: "NextRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_SecondParticipantId",
                table: "Round",
                column: "SecondParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_WinnerParticipantId",
                table: "Round",
                column: "WinnerParticipantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Round");

            migrationBuilder.DropTable(
                name: "Bracket");
        }
    }
}
