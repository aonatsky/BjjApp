using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class ParticipantStatusAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "PaymentApproved",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "Participant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Participant");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Participant",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PaymentApproved",
                table: "Order",
                nullable: false,
                defaultValue: false);
        }
    }
}
