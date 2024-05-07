using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class osakdokasdkoasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_PageContents_PageContentId",
                table: "ImageResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_PageContents_PageContentId",
                table: "ImageResources",
                column: "PageContentId",
                principalTable: "PageContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_PageContents_PageContentId",
                table: "ImageResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_PageContents_PageContentId",
                table: "ImageResources",
                column: "PageContentId",
                principalTable: "PageContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
