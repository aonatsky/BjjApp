using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Migrations
{
    public partial class DisableCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Category_CategoryId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Team_TeamId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Category_CategoryId",
                schema: "trnmnt",
                table: "Fighter",
                column: "CategoryId",
                principalSchema: "trnmnt",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Team_TeamId",
                schema: "trnmnt",
                table: "Fighter",
                column: "TeamId",
                principalSchema: "trnmnt",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter",
                column: "WeightDivisionId",
                principalSchema: "trnmnt",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Category_CategoryId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Team_TeamId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Category_CategoryId",
                schema: "trnmnt",
                table: "Fighter",
                column: "CategoryId",
                principalSchema: "trnmnt",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Team_TeamId",
                schema: "trnmnt",
                table: "Fighter",
                column: "TeamId",
                principalSchema: "trnmnt",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter",
                column: "WeightDivisionId",
                principalSchema: "trnmnt",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
