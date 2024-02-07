using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class contactPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContactPersonId",
                table: "Contents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactPersons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPersons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContactPersonId",
                table: "Contents",
                column: "ContactPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_ContactPersons_ContactPersonId",
                table: "Contents",
                column: "ContactPersonId",
                principalTable: "ContactPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_ContactPersons_ContactPersonId",
                table: "Contents");

            migrationBuilder.DropTable(
                name: "ContactPersons");

            migrationBuilder.DropIndex(
                name: "IX_Contents_ContactPersonId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "ContactPersonId",
                table: "Contents");
        }
    }
}
