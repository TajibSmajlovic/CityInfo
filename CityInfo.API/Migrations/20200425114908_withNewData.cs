using Microsoft.EntityFrameworkCore.Migrations;

namespace CityInfo.API.Migrations
{
    public partial class withNewData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Tamo gdje su piramide.", "Visoko" });

            migrationBuilder.InsertData(
                table: "PointOfInterests",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 1, 1, "Jes piramida svega mi!!!", "Piramida Sunca" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
