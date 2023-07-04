using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class content : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageStrings_News_NewsId",
                table: "LanguageStrings");

            migrationBuilder.DropIndex(
                name: "IX_LanguageStrings_NewsId",
                table: "LanguageStrings");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "LanguageStrings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NewsId",
                table: "LanguageStrings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStrings_NewsId",
                table: "LanguageStrings",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageStrings_News_NewsId",
                table: "LanguageStrings",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
