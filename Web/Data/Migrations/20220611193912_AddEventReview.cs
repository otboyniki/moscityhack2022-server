using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class AddEventReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ReviewScore",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "CompanyRate",
                table: "Review",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Review",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoalComplianceRate",
                table: "Review",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewScore_UserId",
                table: "ReviewScore",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_EventId",
                table: "Review",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Events_EventId",
                table: "Review",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewScore_Users_UserId",
                table: "ReviewScore",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Events_EventId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewScore_Users_UserId",
                table: "ReviewScore");

            migrationBuilder.DropIndex(
                name: "IX_ReviewScore_UserId",
                table: "ReviewScore");

            migrationBuilder.DropIndex(
                name: "IX_Review_EventId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReviewScore");

            migrationBuilder.DropColumn(
                name: "CompanyRate",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "GoalComplianceRate",
                table: "Review");
        }
    }
}
