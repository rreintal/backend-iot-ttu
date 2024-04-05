using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class cascade3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId",
                table: "AppRefreshTokens");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId1",
                table: "AppRefreshTokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppRefreshTokens_AppUserId1",
                table: "AppRefreshTokens",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId",
                table: "AppRefreshTokens",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId1",
                table: "AppRefreshTokens",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId",
                table: "AppRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId1",
                table: "AppRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AppRefreshTokens_AppUserId1",
                table: "AppRefreshTokens");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "AppRefreshTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId",
                table: "AppRefreshTokens",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
