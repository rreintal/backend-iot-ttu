using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LanguageStrings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStrings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageStringTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageCulture = table.Column<string>(type: "text", nullable: false),
                    TranslationValue = table.Column<string>(type: "text", nullable: false),
                    LanguageStringId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStringTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageStringTranslations_LanguageStrings_LanguageStringId",
                        column: x => x.LanguageStringId,
                        principalTable: "LanguageStrings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStringTranslations_LanguageStringId",
                table: "LanguageStringTranslations",
                column: "LanguageStringId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageStringTranslations");

            migrationBuilder.DropTable(
                name: "LanguageStrings");
        }
    }
}
