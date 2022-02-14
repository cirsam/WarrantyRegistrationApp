using Microsoft.EntityFrameworkCore.Migrations;

namespace WarrantyRegistrationApp.Migrations
{
    public partial class UpdateCustomerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "ProductWarrantyDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "ProductWarrantyDatas");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Customers");
        }
    }
}
