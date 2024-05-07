using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class HOMEPAGEBANNERSPKDASOKDOSAKDAS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ImageResources");

            migrationBuilder.AddColumn<Guid>(
                name: "HomePageBannerId",
                table: "ImageResources",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerImageId",
                table: "ImageResources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_HomePageBannerId",
                table: "ImageResources",
                column: "HomePageBannerId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_PartnerImageId",
                table: "ImageResources",
                column: "PartnerImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_HomePageBanners_HomePageBannerId",
                table: "ImageResources",
                column: "HomePageBannerId",
                principalTable: "HomePageBanners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_PartnerImages_PartnerImageId",
                table: "ImageResources",
                column: "PartnerImageId",
                principalTable: "PartnerImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_HomePageBanners_HomePageBannerId",
                table: "ImageResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_PartnerImages_PartnerImageId",
                table: "ImageResources");

            migrationBuilder.DropIndex(
                name: "IX_ImageResources_HomePageBannerId",
                table: "ImageResources");

            migrationBuilder.DropIndex(
                name: "IX_ImageResources_PartnerImageId",
                table: "ImageResources");

            migrationBuilder.DropColumn(
                name: "HomePageBannerId",
                table: "ImageResources");

            migrationBuilder.DropColumn(
                name: "PartnerImageId",
                table: "ImageResources");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ImageResources",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
