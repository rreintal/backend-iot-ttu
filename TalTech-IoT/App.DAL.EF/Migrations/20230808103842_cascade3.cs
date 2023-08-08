using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class cascade3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_LanguageStrings_LanguageStringId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_News_NewsId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_LanguageStringTranslations_LanguageStrings_LanguageStringId",
                table: "LanguageStringTranslations");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_LanguageStrings_LanguageStringId",
                table: "Contents",
                column: "LanguageStringId",
                principalTable: "LanguageStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_News_NewsId",
                table: "Contents",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageStringTranslations_LanguageStrings_LanguageStringId",
                table: "LanguageStringTranslations",
                column: "LanguageStringId",
                principalTable: "LanguageStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_LanguageStrings_LanguageStringId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_News_NewsId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_LanguageStringTranslations_LanguageStrings_LanguageStringId",
                table: "LanguageStringTranslations");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageStringTranslations_LanguageStrings_LanguageStringId",
                table: "LanguageStringTranslations",
                column: "LanguageStringId",
                principalTable: "LanguageStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
