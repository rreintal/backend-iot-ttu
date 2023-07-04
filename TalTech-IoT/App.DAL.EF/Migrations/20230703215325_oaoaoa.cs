using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class oaoaoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageStrings_LanguageStringTypes_LanguageStringTypeId",
                table: "LanguageStrings");

            migrationBuilder.DropTable(
                name: "LanguageStringTypes");

            migrationBuilder.DropIndex(
                name: "IX_LanguageStrings_LanguageStringTypeId",
                table: "LanguageStrings");

            migrationBuilder.DropColumn(
                name: "LanguageStringTypeId",
                table: "LanguageStrings");

            migrationBuilder.AddColumn<Guid>(
                name: "ContentId",
                table: "LanguageStrings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContentType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: true),
                    LanguageStringId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Content_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Content_LanguageStrings_LanguageStringId",
                        column: x => x.LanguageStringId,
                        principalTable: "LanguageStrings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Content_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Content_ContentTypeId",
                table: "Content",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Content_LanguageStringId",
                table: "Content",
                column: "LanguageStringId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Content_NewsId",
                table: "Content",
                column: "NewsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "ContentType");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "LanguageStrings");

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageStringTypeId",
                table: "LanguageStrings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LanguageStringTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStringTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStrings_LanguageStringTypeId",
                table: "LanguageStrings",
                column: "LanguageStringTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageStrings_LanguageStringTypes_LanguageStringTypeId",
                table: "LanguageStrings",
                column: "LanguageStringTypeId",
                principalTable: "LanguageStringTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
