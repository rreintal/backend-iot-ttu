using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class Fapsdlpasdsal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FeedPagePostId",
                table: "ImageResources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_FeedPagePostId",
                table: "ImageResources",
                column: "FeedPagePostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_FeedPagePosts_FeedPagePostId",
                table: "ImageResources",
                column: "FeedPagePostId",
                principalTable: "FeedPagePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_FeedPagePosts_FeedPagePostId",
                table: "ImageResources");

            migrationBuilder.DropIndex(
                name: "IX_ImageResources_FeedPagePostId",
                table: "ImageResources");

            migrationBuilder.DropColumn(
                name: "FeedPagePostId",
                table: "ImageResources");
        }
    }
}
