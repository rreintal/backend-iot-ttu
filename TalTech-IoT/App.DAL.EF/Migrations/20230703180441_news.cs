using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class news : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LanguageStringTypeId",
                table: "LanguageStrings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "NewsId",
                table: "LanguageStrings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LanguageStringTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStringTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStrings_LanguageStringTypeId",
                table: "LanguageStrings",
                column: "LanguageStringTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStrings_NewsId",
                table: "LanguageStrings",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageStrings_LanguageStringTypes_LanguageStringTypeId",
                table: "LanguageStrings",
                column: "LanguageStringTypeId",
                principalTable: "LanguageStringTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageStrings_News_NewsId",
                table: "LanguageStrings",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageStrings_LanguageStringTypes_LanguageStringTypeId",
                table: "LanguageStrings");

            migrationBuilder.DropForeignKey(
                name: "FK_LanguageStrings_News_NewsId",
                table: "LanguageStrings");

            migrationBuilder.DropTable(
                name: "LanguageStringTypes");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropIndex(
                name: "IX_LanguageStrings_LanguageStringTypeId",
                table: "LanguageStrings");

            migrationBuilder.DropIndex(
                name: "IX_LanguageStrings_NewsId",
                table: "LanguageStrings");

            migrationBuilder.DropColumn(
                name: "LanguageStringTypeId",
                table: "LanguageStrings");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "LanguageStrings");
        }
    }
}
