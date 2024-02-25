using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class soos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_Contents_ContentId",
                table: "ImageResources");

            migrationBuilder.DropIndex(
                name: "IX_ImageResources_ContentId",
                table: "ImageResources");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "ImageResources");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContentId",
                table: "ImageResources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageResources_ContentId",
                table: "ImageResources",
                column: "ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_Contents_ContentId",
                table: "ImageResources",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
