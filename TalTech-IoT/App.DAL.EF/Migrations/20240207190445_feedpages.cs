using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class feedpages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FeedPageCategoryId",
                table: "Contents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FeedPagePostId",
                table: "Contents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FeedPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FeedPageName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedPageCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FeedPageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPageCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedPageCategories_FeedPages_FeedPageId",
                        column: x => x.FeedPageId,
                        principalTable: "FeedPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedPagePosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FeedPageCategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPagePosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedPagePosts_FeedPageCategories_FeedPageCategoryId",
                        column: x => x.FeedPageCategoryId,
                        principalTable: "FeedPageCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_FeedPageCategoryId",
                table: "Contents",
                column: "FeedPageCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_FeedPagePostId",
                table: "Contents",
                column: "FeedPagePostId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPageCategories_FeedPageId",
                table: "FeedPageCategories",
                column: "FeedPageId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPagePosts_FeedPageCategoryId",
                table: "FeedPagePosts",
                column: "FeedPageCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FeedPageCategories_FeedPageCategoryId",
                table: "Contents",
                column: "FeedPageCategoryId",
                principalTable: "FeedPageCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FeedPagePosts_FeedPagePostId",
                table: "Contents",
                column: "FeedPagePostId",
                principalTable: "FeedPagePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FeedPageCategories_FeedPageCategoryId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FeedPagePosts_FeedPagePostId",
                table: "Contents");

            migrationBuilder.DropTable(
                name: "FeedPagePosts");

            migrationBuilder.DropTable(
                name: "FeedPageCategories");

            migrationBuilder.DropTable(
                name: "FeedPages");

            migrationBuilder.DropIndex(
                name: "IX_Contents_FeedPageCategoryId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_FeedPagePostId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "FeedPageCategoryId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "FeedPagePostId",
                table: "Contents");
        }
    }
}
