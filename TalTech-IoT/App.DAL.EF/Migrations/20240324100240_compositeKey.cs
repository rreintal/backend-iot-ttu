using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class compositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageStringTranslations",
                table: "LanguageStringTranslations");

            migrationBuilder.DropIndex(
                name: "IX_LanguageStringTranslations_LanguageStringId",
                table: "LanguageStringTranslations");

            migrationBuilder.DropIndex(
                name: "IX_LanguageStrings_Value",
                table: "LanguageStrings");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "LanguageStrings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageStringTranslations",
                table: "LanguageStringTranslations",
                columns: new[] { "LanguageStringId", "LanguageCulture" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageStringTranslations",
                table: "LanguageStringTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "LanguageStrings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageStringTranslations",
                table: "LanguageStringTranslations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStringTranslations_LanguageStringId",
                table: "LanguageStringTranslations",
                column: "LanguageStringId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStrings_Value",
                table: "LanguageStrings",
                column: "Value",
                unique: true,
                filter: "\"TopicAreaId\" IS NOT NULL");
        }
    }
}
