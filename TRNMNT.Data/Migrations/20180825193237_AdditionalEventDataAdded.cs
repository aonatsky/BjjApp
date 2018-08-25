using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class AdditionalEventDataAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BracketsPublished",
                table: "Event",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CorrectionsEnabled",
                table: "Event",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ParticipantListsPublished",
                table: "Event",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BracketsPublished",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "CorrectionsEnabled",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ParticipantListsPublished",
                table: "Event");
        }
    }
}
