using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class oksaodkasokd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_Projects_ProjectId",
                table: "ImageResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_Projects_ProjectId",
                table: "ImageResources",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageResources_Projects_ProjectId",
                table: "ImageResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageResources_Projects_ProjectId",
                table: "ImageResources",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
