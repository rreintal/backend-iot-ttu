using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class ososso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FeedPagePosts_FeedPagePostId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_PageContents_PageIdentifier",
                table: "PageContents");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPages_FeedPageName",
                table: "FeedPages",
                column: "FeedPageName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FeedPagePosts_FeedPagePostId",
                table: "Contents",
                column: "FeedPagePostId",
                principalTable: "FeedPagePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FeedPagePosts_FeedPagePostId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_FeedPages_FeedPageName",
                table: "FeedPages");

            migrationBuilder.CreateIndex(
                name: "IX_PageContents_PageIdentifier",
                table: "PageContents",
                column: "PageIdentifier",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FeedPagePosts_FeedPagePostId",
                table: "Contents",
                column: "FeedPagePostId",
                principalTable: "FeedPagePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
