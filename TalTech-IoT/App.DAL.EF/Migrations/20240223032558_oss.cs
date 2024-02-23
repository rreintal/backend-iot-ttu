using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class oss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OpenSourceSolutionId",
                table: "Contents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OpenSourceSolutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Private = table.Column<bool>(type: "boolean", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenSourceSolutions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_OpenSourceSolutionId",
                table: "Contents",
                column: "OpenSourceSolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_OpenSourceSolutions_OpenSourceSolutionId",
                table: "Contents",
                column: "OpenSourceSolutionId",
                principalTable: "OpenSourceSolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_OpenSourceSolutions_OpenSourceSolutionId",
                table: "Contents");

            migrationBuilder.DropTable(
                name: "OpenSourceSolutions");

            migrationBuilder.DropIndex(
                name: "IX_Contents_OpenSourceSolutionId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "OpenSourceSolutionId",
                table: "Contents");
        }
    }
}
