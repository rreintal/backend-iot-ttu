using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class aaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_ContentType_ContentTypeId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_LanguageStrings_LanguageStringId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_News_NewsId",
                table: "Content");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentType",
                table: "ContentType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Content",
                table: "Content");

            migrationBuilder.RenameTable(
                name: "ContentType",
                newName: "ContentTypes");

            migrationBuilder.RenameTable(
                name: "Content",
                newName: "Contents");

            migrationBuilder.RenameIndex(
                name: "IX_Content_NewsId",
                table: "Contents",
                newName: "IX_Contents_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_LanguageStringId",
                table: "Contents",
                newName: "IX_Contents_LanguageStringId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_ContentTypeId",
                table: "Contents",
                newName: "IX_Contents_ContentTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentTypes",
                table: "ContentTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contents",
                table: "Contents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_ContentTypes_ContentTypeId",
                table: "Contents",
                column: "ContentTypeId",
                principalTable: "ContentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_LanguageStrings_LanguageStringId",
                table: "Contents",
                column: "LanguageStringId",
                principalTable: "LanguageStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_News_NewsId",
                table: "Contents",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_ContentTypes_ContentTypeId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_LanguageStrings_LanguageStringId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_News_NewsId",
                table: "Contents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentTypes",
                table: "ContentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contents",
                table: "Contents");

            migrationBuilder.RenameTable(
                name: "ContentTypes",
                newName: "ContentType");

            migrationBuilder.RenameTable(
                name: "Contents",
                newName: "Content");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_NewsId",
                table: "Content",
                newName: "IX_Content_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_LanguageStringId",
                table: "Content",
                newName: "IX_Content_LanguageStringId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_ContentTypeId",
                table: "Content",
                newName: "IX_Content_ContentTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentType",
                table: "ContentType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Content",
                table: "Content",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_ContentType_ContentTypeId",
                table: "Content",
                column: "ContentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_LanguageStrings_LanguageStringId",
                table: "Content",
                column: "LanguageStringId",
                principalTable: "LanguageStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Content_News_NewsId",
                table: "Content",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
