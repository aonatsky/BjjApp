using Microsoft.EntityFrameworkCore.Migrations;

namespace TRNMNT.Data.Migrations
{
    public partial class FederationApprovalAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FederationApprovalStatus",
                table: "Team",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FederationApprovalStatus",
                table: "Team");
        }
    }
}
