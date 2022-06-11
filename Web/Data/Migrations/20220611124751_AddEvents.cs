using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class AddEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Events_EventId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "IsRegisteredVolunteersNeeded",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MaxVolunteersNumber",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MinVolunteersNumber",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Participants",
                newName: "EventSpecializationId");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_EventId",
                table: "Participants",
                newName: "IX_Participants_EventSpecializationId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizerUserId",
                table: "Participants",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventSpecializations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Requirements = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinVolunteersNumber = table.Column<int>(type: "integer", nullable: false),
                    MaxVolunteersNumber = table.Column<int>(type: "integer", nullable: false),
                    IsRegisteredVolunteersNeeded = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSpecializations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSpecializations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_OrganizerUserId",
                table: "Participants",
                column: "OrganizerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSpecializations_EventId",
                table: "EventSpecializations",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_EventSpecializations_EventSpecializationId",
                table: "Participants",
                column: "EventSpecializationId",
                principalTable: "EventSpecializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_OrganizerUserId",
                table: "Participants",
                column: "OrganizerUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_EventSpecializations_EventSpecializationId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_OrganizerUserId",
                table: "Participants");

            migrationBuilder.DropTable(
                name: "EventSpecializations");

            migrationBuilder.DropIndex(
                name: "IX_Participants_OrganizerUserId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "OrganizerUserId",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "EventSpecializationId",
                table: "Participants",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_EventSpecializationId",
                table: "Participants",
                newName: "IX_Participants_EventId");

            migrationBuilder.AddColumn<bool>(
                name: "IsRegisteredVolunteersNeeded",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxVolunteersNumber",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinVolunteersNumber",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Events_EventId",
                table: "Participants",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
