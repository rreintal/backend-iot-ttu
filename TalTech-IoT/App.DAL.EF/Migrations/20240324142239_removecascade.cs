using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class removecascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId",
                table: "AppRefreshTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId",
                table: "AppRefreshTokens",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId",
                table: "AppRefreshTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRefreshTokens_AspNetUsers_AppUserId",
                table: "AppRefreshTokens",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
