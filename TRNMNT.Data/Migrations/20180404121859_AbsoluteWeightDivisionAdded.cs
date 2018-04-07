using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TRNMNT.Data.Migrations
{
    public partial class AbsoluteWeightDivisionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAbsolute",
                table: "WeightDivision",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "AbsoluteWeightDivisionId",
                table: "Participant",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participant_AbsoluteWeightDivisionId",
                table: "Participant",
                column: "AbsoluteWeightDivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_WeightDivision_AbsoluteWeightDivisionId",
                table: "Participant",
                column: "AbsoluteWeightDivisionId",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participant_WeightDivision_AbsoluteWeightDivisionId",
                table: "Participant");

            migrationBuilder.DropIndex(
                name: "IX_Participant_AbsoluteWeightDivisionId",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "IsAbsolute",
                table: "WeightDivision");

            migrationBuilder.DropColumn(
                name: "AbsoluteWeightDivisionId",
                table: "Participant");
        }
    }
}
