using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class pagecontent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PageContentId",
                table: "Contents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PageContentId1",
                table: "Contents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PageContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PageIdentifier = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageContents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PageContentId",
                table: "Contents",
                column: "PageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PageContentId1",
                table: "Contents",
                column: "PageContentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_PageContents_PageContentId",
                table: "Contents",
                column: "PageContentId",
                principalTable: "PageContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_PageContents_PageContentId1",
                table: "Contents",
                column: "PageContentId1",
                principalTable: "PageContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_PageContents_PageContentId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_PageContents_PageContentId1",
                table: "Contents");

            migrationBuilder.DropTable(
                name: "PageContents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_PageContentId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_PageContentId1",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "PageContentId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "PageContentId1",
                table: "Contents");
        }
    }
}
