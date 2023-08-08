using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class cascade4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HasTopicAreas_News_NewsId",
                table: "HasTopicAreas");

            migrationBuilder.AddForeignKey(
                name: "FK_HasTopicAreas_News_NewsId",
                table: "HasTopicAreas",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HasTopicAreas_News_NewsId",
                table: "HasTopicAreas");

            migrationBuilder.AddForeignKey(
                name: "FK_HasTopicAreas_News_NewsId",
                table: "HasTopicAreas",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
