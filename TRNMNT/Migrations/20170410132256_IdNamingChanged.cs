using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Migrations
{
    public partial class IdNamingChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Category_CategoryID",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Team_TeamID",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.RenameColumn(
                name: "WeightDivisionID",
                schema: "trnmnt",
                table: "WeightDivision",
                newName: "WeightDivisionId");

            migrationBuilder.RenameColumn(
                name: "TeamID",
                schema: "trnmnt",
                table: "Team",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "TeamID",
                schema: "trnmnt",
                table: "Fighter",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                schema: "trnmnt",
                table: "Fighter",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "FighterID",
                schema: "trnmnt",
                table: "Fighter",
                newName: "FighterId");

            migrationBuilder.RenameIndex(
                name: "IX_Fighter_TeamID",
                schema: "trnmnt",
                table: "Fighter",
                newName: "IX_Fighter_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Fighter_CategoryID",
                schema: "trnmnt",
                table: "Fighter",
                newName: "IX_Fighter_CategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                schema: "trnmnt",
                table: "Category",
                newName: "CategoryId");

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
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.RenameColumn(
                name: "WeightDivisionId",
                schema: "trnmnt",
                table: "WeightDivision",
                newName: "WeightDivisionID");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                schema: "trnmnt",
                table: "Team",
                newName: "TeamID");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                schema: "trnmnt",
                table: "Fighter",
                newName: "TeamID");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                schema: "trnmnt",
                table: "Fighter",
                newName: "CategoryID");

            migrationBuilder.RenameColumn(
                name: "FighterId",
                schema: "trnmnt",
                table: "Fighter",
                newName: "FighterID");

            migrationBuilder.RenameIndex(
                name: "IX_Fighter_TeamId",
                schema: "trnmnt",
                table: "Fighter",
                newName: "IX_Fighter_TeamID");

            migrationBuilder.RenameIndex(
                name: "IX_Fighter_CategoryId",
                schema: "trnmnt",
                table: "Fighter",
                newName: "IX_Fighter_CategoryID");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                schema: "trnmnt",
                table: "Category",
                newName: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Category_CategoryID",
                schema: "trnmnt",
                table: "Fighter",
                column: "CategoryID",
                principalSchema: "trnmnt",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Team_TeamID",
                schema: "trnmnt",
                table: "Fighter",
                column: "TeamID",
                principalSchema: "trnmnt",
                principalTable: "Team",
                principalColumn: "TeamID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
