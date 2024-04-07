using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class sokadoskad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_HomePageBanners_HomePageBannerId",
                table: "ImageResources");

            migrationBuilder.DropIndex(
                name: "IX_ImageResources_HomePageBannerId",
                table: "ImageResources");

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_HomePageBannerId",
                table: "ImageResources",
                column: "HomePageBannerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_HomePageBanners_HomePageBannerId",
                table: "ImageResources",
                column: "HomePageBannerId",
                principalTable: "HomePageBanners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_HomePageBanners_HomePageBannerId",
                table: "ImageResources");

            migrationBuilder.DropIndex(
                name: "IX_ImageResources_HomePageBannerId",
                table: "ImageResources");

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_HomePageBannerId",
                table: "ImageResources",
                column: "HomePageBannerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_HomePageBanners_HomePageBannerId",
                table: "ImageResources",
                column: "HomePageBannerId",
                principalTable: "HomePageBanners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
