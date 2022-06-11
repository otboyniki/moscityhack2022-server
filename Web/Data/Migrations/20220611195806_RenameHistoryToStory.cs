using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class RenameHistoryToStory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Histories_HistoryId",
                table: "Review");

            migrationBuilder.DropTable(
                name: "HistoryActivity");

            migrationBuilder.DropTable(
                name: "HistoryScore");

            migrationBuilder.DropTable(
                name: "HistoryView");

            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.RenameColumn(
                name: "HistoryId",
                table: "Review",
                newName: "StoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_HistoryId",
                table: "Review",
                newName: "IX_Review_StoryId");

            migrationBuilder.CreateTable(
                name: "Stories",
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
                    table.PrimaryKey("PK_Stories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stories_Files_PreviewId",
                        column: x => x.PreviewId,
                        principalTable: "Files",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoryActivity",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryActivity", x => new { x.StoryId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_StoryActivity_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoryActivity_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryScore",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Positive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryScore", x => new { x.UserId, x.StoryId });
                    table.ForeignKey(
                        name: "FK_StoryScore_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoryScore_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryView",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryView", x => new { x.UserId, x.StoryId });
                    table.ForeignKey(
                        name: "FK_StoryView_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoryView_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stories_CompanyId",
                table: "Stories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_PreviewId",
                table: "Stories",
                column: "PreviewId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryActivity_ActivityId",
                table: "StoryActivity",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryScore_StoryId",
                table: "StoryScore",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryView_StoryId",
                table: "StoryView",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Stories_StoryId",
                table: "Review",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Stories_StoryId",
                table: "Review");

            migrationBuilder.DropTable(
                name: "StoryActivity");

            migrationBuilder.DropTable(
                name: "StoryScore");

            migrationBuilder.DropTable(
                name: "StoryView");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "Review",
                newName: "HistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_StoryId",
                table: "Review",
                newName: "IX_Review_HistoryId");

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviewId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Format = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
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
                    HistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "IX_HistoryScore_HistoryId",
                table: "HistoryScore",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryView_HistoryId",
                table: "HistoryView",
                column: "HistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Histories_HistoryId",
                table: "Review",
                column: "HistoryId",
                principalTable: "Histories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
