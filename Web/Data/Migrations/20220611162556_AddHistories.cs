using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class AddHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Confirmed",
                table: "Participants",
                newName: "IsConfirmed");

            migrationBuilder.AddColumn<bool>(
                name: "IsAttended",
                table: "Participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Format = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviewId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Histories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Histories_Files_PreviewId",
                        column: x => x.PreviewId,
                        principalTable: "Files",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoryActivity",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryActivity", x => new { x.HistoryId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_HistoryActivity_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryActivity_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryComment_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryComment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryScore",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Positive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryScore", x => new { x.UserId, x.HistoryId });
                    table.ForeignKey(
                        name: "FK_HistoryScore_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryScore_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryView",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryView", x => new { x.UserId, x.HistoryId });
                    table.ForeignKey(
                        name: "FK_HistoryView_Histories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryView_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryCommentScore",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoryCommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Positive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryCommentScore", x => new { x.UserId, x.HistoryCommentId });
                    table.ForeignKey(
                        name: "FK_HistoryCommentScore_HistoryComment_HistoryCommentId",
                        column: x => x.HistoryCommentId,
                        principalTable: "HistoryComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryCommentScore_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Histories_CompanyId",
                table: "Histories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_PreviewId",
                table: "Histories",
                column: "PreviewId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryActivity_ActivityId",
                table: "HistoryActivity",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryComment_HistoryId",
                table: "HistoryComment",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryComment_UserId",
                table: "HistoryComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryCommentScore_HistoryCommentId",
                table: "HistoryCommentScore",
                column: "HistoryCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryScore_HistoryId",
                table: "HistoryScore",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryView_HistoryId",
                table: "HistoryView",
                column: "HistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryActivity");

            migrationBuilder.DropTable(
                name: "HistoryCommentScore");

            migrationBuilder.DropTable(
                name: "HistoryScore");

            migrationBuilder.DropTable(
                name: "HistoryView");

            migrationBuilder.DropTable(
                name: "HistoryComment");

            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropColumn(
                name: "IsAttended",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "IsConfirmed",
                table: "Participants",
                newName: "Confirmed");
        }
    }
}
