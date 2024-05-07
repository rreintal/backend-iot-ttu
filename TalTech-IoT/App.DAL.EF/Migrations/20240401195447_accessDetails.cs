using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class accessDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OpenSourceSolutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessDetails_OpenSourceSolutions_OpenSourceSolutionId",
                        column: x => x.OpenSourceSolutionId,
                        principalTable: "OpenSourceSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessDetails_OpenSourceSolutionId",
                table: "AccessDetails",
                column: "OpenSourceSolutionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessDetails");
        }
    }
}
