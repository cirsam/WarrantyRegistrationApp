using Microsoft.EntityFrameworkCore.Migrations;

namespace WarrantyRegistrationApp.Migrations
{
    public partial class CapitalizeWarrantyDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "warrantyDate",
                table: "ProductWarrantyDatas",
                newName: "WarrantyDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WarrantyDate",
                table: "ProductWarrantyDatas",
                newName: "warrantyDate");
        }
    }
}
