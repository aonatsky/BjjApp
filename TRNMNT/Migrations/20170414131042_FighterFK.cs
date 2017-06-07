using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Web.Migrations
{
    public partial class FighterFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Category_CategoryID",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                schema: "trnmnt",
                table: "Fighter",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Fighter_CategoryID",
                schema: "trnmnt",
                table: "Fighter",
                newName: "IX_Fighter_CategoryId");

            migrationBuilder.AddColumn<Guid>(
                name: "WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Fighter_WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter",
                column: "WeightDivisionId");

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
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter",
                column: "WeightDivisionId",
                principalSchema: "trnmnt",
                principalTable: "WeightDivision",
                principalColumn: "WeightDivisionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_Category_CategoryId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighter_WeightDivision_WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.DropIndex(
                name: "IX_Fighter_WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.DropColumn(
                name: "WeightDivisionId",
                schema: "trnmnt",
                table: "Fighter");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                schema: "trnmnt",
                table: "Fighter",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Fighter_CategoryId",
                schema: "trnmnt",
                table: "Fighter",
                newName: "IX_Fighter_CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fighter_Category_CategoryID",
                schema: "trnmnt",
                table: "Fighter",
                column: "CategoryID",
                principalSchema: "trnmnt",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
