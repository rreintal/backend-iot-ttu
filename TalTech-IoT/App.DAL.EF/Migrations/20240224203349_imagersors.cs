using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class imagersors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResource_Contents_ContentId",
                table: "ImageResource");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageResource_News_NewsId",
                table: "ImageResource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageResource",
                table: "ImageResource");

            migrationBuilder.RenameTable(
                name: "ImageResource",
                newName: "ImageResources");

            migrationBuilder.RenameIndex(
                name: "IX_ImageResource_NewsId",
                table: "ImageResources",
                newName: "IX_ImageResources_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_ImageResource_ContentId",
                table: "ImageResources",
                newName: "IX_ImageResources_ContentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageResources",
                table: "ImageResources",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_Contents_ContentId",
                table: "ImageResources",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_News_NewsId",
                table: "ImageResources",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_Contents_ContentId",
                table: "ImageResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_News_NewsId",
                table: "ImageResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageResources",
                table: "ImageResources");

            migrationBuilder.RenameTable(
                name: "ImageResources",
                newName: "ImageResource");

            migrationBuilder.RenameIndex(
                name: "IX_ImageResources_NewsId",
                table: "ImageResource",
                newName: "IX_ImageResource_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_ImageResources_ContentId",
                table: "ImageResource",
                newName: "IX_ImageResource_ContentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageResource",
                table: "ImageResource",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResource_Contents_ContentId",
                table: "ImageResource",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResource_News_NewsId",
                table: "ImageResource",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
