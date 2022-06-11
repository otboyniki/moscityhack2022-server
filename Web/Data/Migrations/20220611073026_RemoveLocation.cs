using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Web.Migrations
{
    public partial class RemoveLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Users",
                type: "geography (point)",
                nullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Events",
                type: "geography (point)",
                nullable: false);
        }
    }
}
