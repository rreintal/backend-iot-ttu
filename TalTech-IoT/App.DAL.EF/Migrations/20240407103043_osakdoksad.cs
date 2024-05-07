using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class osakdoksad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_FeedPagePosts_FeedPagePostId",
                table: "ImageResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_FeedPagePosts_FeedPagePostId",
                table: "ImageResources",
                column: "FeedPagePostId",
                principalTable: "FeedPagePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_FeedPagePosts_FeedPagePostId",
                table: "ImageResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_FeedPagePosts_FeedPagePostId",
                table: "ImageResources",
                column: "FeedPagePostId",
                principalTable: "FeedPagePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
