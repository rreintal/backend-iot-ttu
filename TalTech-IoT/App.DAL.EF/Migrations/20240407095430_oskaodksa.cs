using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class oskaodksa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_PartnerImages_PartnerImageId",
                table: "ImageResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_PartnerImages_PartnerImageId",
                table: "ImageResources",
                column: "PartnerImageId",
                principalTable: "PartnerImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_PartnerImages_PartnerImageId",
                table: "ImageResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_PartnerImages_PartnerImageId",
                table: "ImageResources",
                column: "PartnerImageId",
                principalTable: "PartnerImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
