using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class imageresources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PageContentId",
                table: "ImageResources",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ImageResources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_PageContentId",
                table: "ImageResources",
                column: "PageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_ProjectId",
                table: "ImageResources",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_PageContents_PageContentId",
                table: "ImageResources",
                column: "PageContentId",
                principalTable: "PageContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_Projects_ProjectId",
                table: "ImageResources",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_PageContents_PageContentId",
                table: "ImageResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_Projects_ProjectId",
                table: "ImageResources");

            migrationBuilder.DropIndex(
                name: "IX_ImageResources_PageContentId",
                table: "ImageResources");

            migrationBuilder.DropIndex(
                name: "IX_ImageResources_ProjectId",
                table: "ImageResources");

            migrationBuilder.DropColumn(
                name: "PageContentId",
                table: "ImageResources");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ImageResources");
        }
    }
}
