using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class AlterEventsAddActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Events");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityId",
                table: "Events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql($@"insert into ""Activities"" (""Id"", ""Title"") values ('00000000-0000-0000-0000-000000000000', 'Волонтёрство')");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ActivityId",
                table: "Events",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Activities_ActivityId",
                table: "Events",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Activities_ActivityId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ActivityId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "Kind",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
