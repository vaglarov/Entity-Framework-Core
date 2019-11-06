using Microsoft.EntityFrameworkCore.Migrations;

namespace LabCarsData.Migrations
{
    public partial class UniqueValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Makes_Name",
                table: "Makes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_VIN",
                table: "Cars",
                column: "VIN",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Makes_Name",
                table: "Makes");

            migrationBuilder.DropIndex(
                name: "IX_Cars_VIN",
                table: "Cars");
        }
    }
}
