using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class AlterEventSpecs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ages_From",
                table: "EventSpecializations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ages_To",
                table: "EventSpecializations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "EventSpecializations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ages_From",
                table: "EventSpecializations");

            migrationBuilder.DropColumn(
                name: "Ages_To",
                table: "EventSpecializations");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "EventSpecializations");
        }
    }
}
