using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class homepagebanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HomePageBannerId",
                table: "Contents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HomePageBanners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageBanners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_HomePageBannerId",
                table: "Contents",
                column: "HomePageBannerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_HomePageBanners_HomePageBannerId",
                table: "Contents",
                column: "HomePageBannerId",
                principalTable: "HomePageBanners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_HomePageBanners_HomePageBannerId",
                table: "Contents");

            migrationBuilder.DropTable(
                name: "HomePageBanners");

            migrationBuilder.DropIndex(
                name: "IX_Contents_HomePageBannerId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "HomePageBannerId",
                table: "Contents");
        }
    }
}
