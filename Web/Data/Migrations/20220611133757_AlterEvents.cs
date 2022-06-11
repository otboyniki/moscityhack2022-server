using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web.Migrations
{
    public partial class AlterEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_OrganizerUserId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_OrganizerUserId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "OrganizerUserId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Users",
                newName: "Address_PointLocation");

            migrationBuilder.RenameColumn(
                name: "Until",
                table: "Events",
                newName: "Recruitment_Until");

            migrationBuilder.RenameColumn(
                name: "Since",
                table: "Events",
                newName: "Recruitment_Since");

            migrationBuilder.AddColumn<string>(
                name: "Address_StringLocation",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "EventSpecializations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "EventSpecializations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Recruitment_Until",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Recruitment_Since",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "MeetingNote",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Meeting_Since",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Meeting_Until",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "PreviewId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Terms",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Events_Locations",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PointLocation = table.Column<Point>(type: "geography (point)", nullable: true),
                    StringLocation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events_Locations", x => new { x.EventId, x.Id });
                    table.ForeignKey(
                        name: "FK_Events_Locations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_PreviewId",
                table: "Events",
                column: "PreviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Files_PreviewId",
                table: "Events",
                column: "PreviewId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Files_PreviewId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Events_Locations");

            migrationBuilder.DropIndex(
                name: "IX_Events_PreviewId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Address_StringLocation",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "EventSpecializations");

            migrationBuilder.DropColumn(
                name: "MeetingNote",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Meeting_Since",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Meeting_Until",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PreviewId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Terms",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Address_PointLocation",
                table: "Users",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Recruitment_Until",
                table: "Events",
                newName: "Until");

            migrationBuilder.RenameColumn(
                name: "Recruitment_Since",
                table: "Events",
                newName: "Since");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizerUserId",
                table: "Participants",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "EventSpecializations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Until",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Since",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Events",
                type: "geography (point)",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_OrganizerUserId",
                table: "Participants",
                column: "OrganizerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_OrganizerUserId",
                table: "Participants",
                column: "OrganizerUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
