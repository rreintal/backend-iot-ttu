using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class cascadedelfrofeedpcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FeedPageCategories_FeedPageCategoryId",
                table: "Contents");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FeedPageCategories_FeedPageCategoryId",
                table: "Contents",
                column: "FeedPageCategoryId",
                principalTable: "FeedPageCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FeedPageCategories_FeedPageCategoryId",
                table: "Contents");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FeedPageCategories_FeedPageCategoryId",
                table: "Contents",
                column: "FeedPageCategoryId",
                principalTable: "FeedPageCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
