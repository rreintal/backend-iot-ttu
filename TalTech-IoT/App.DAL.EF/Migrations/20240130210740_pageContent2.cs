using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class pageContent2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_PageContents_PageContentId1",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_PageContentId1",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "PageContentId1",
                table: "Contents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PageContentId1",
                table: "Contents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PageContentId1",
                table: "Contents",
                column: "PageContentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_PageContents_PageContentId1",
                table: "Contents",
                column: "PageContentId1",
                principalTable: "PageContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
