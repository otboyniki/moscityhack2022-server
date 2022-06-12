using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class AddUserNullableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "SocialNetworks",
                table: "Users",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string[]>(
                name: "Languages",
                table: "Users",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "SocialNetworks",
                table: "Users",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string[]>(
                name: "Languages",
                table: "Users",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);
        }
    }
}
