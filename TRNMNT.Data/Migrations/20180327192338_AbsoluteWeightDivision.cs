using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TRNMNT.Data.Migrations
{
    public partial class AbsoluteWeightDivision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participant_WeightDivision_WeightDivisionId",
                table: "Participant");

            migrationBuilder.DropIndex(
                name: "IX_Participant_WeightDivisionId",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "WeightDivisionId",
                table: "Participant");

            migrationBuilder.AddColumn<bool>(
                name: "IsAbsolute",
                table: "WeightDivision",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ParticipantWeightDivision",
                columns: table => new
                {
                    WeightDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantWeightDivision", x => new { x.WeightDivisionId, x.ParticipantId });
                    table.UniqueConstraint("AK_ParticipantWeightDivision_ParticipantId_WeightDivisionId", x => new { x.ParticipantId, x.WeightDivisionId });
                    table.ForeignKey(
                        name: "FK_ParticipantWeightDivision_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParticipantWeightDivision_WeightDivision_WeightDivisionId",
                        column: x => x.WeightDivisionId,
                        principalTable: "WeightDivision",
                        principalColumn: "WeightDivisionId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantWeightDivision");

            migrationBuilder.DropColumn(
                name: "IsAbsolute",
                table: "WeightDivision");

            migrationBuilder.AddColumn<Guid>(
                name: "WeightDivisionId",
                table: "Participant",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Participant_WeightDivisionId",
                table: "Participant",
                column: "WeightDivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_WeightDivision_WeightDivisionId",
                table: "Participant",
                column: "WeightDivisionId",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
