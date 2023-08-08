using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class cascade7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Projects_ProjectId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_HasTopicAreas_Projects_ProjectId",
                table: "HasTopicAreas");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Projects_ProjectId",
                table: "Contents",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HasTopicAreas_Projects_ProjectId",
                table: "HasTopicAreas",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Projects_ProjectId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_HasTopicAreas_Projects_ProjectId",
                table: "HasTopicAreas");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Projects_ProjectId",
                table: "Contents",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HasTopicAreas_Projects_ProjectId",
                table: "HasTopicAreas",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
