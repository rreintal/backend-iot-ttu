using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class hpbcascadedel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_HomePageBanners_HomePageBannerId",
                table: "Contents");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_HomePageBanners_HomePageBannerId",
                table: "Contents",
                column: "HomePageBannerId",
                principalTable: "HomePageBanners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_HomePageBanners_HomePageBannerId",
                table: "Contents");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_HomePageBanners_HomePageBannerId",
                table: "Contents",
                column: "HomePageBannerId",
                principalTable: "HomePageBanners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
