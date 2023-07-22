using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class topicarea2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TopicAreaId",
                table: "LanguageStrings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TopicArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicAreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    LanguageStringId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicArea_LanguageStrings_LanguageStringId",
                        column: x => x.LanguageStringId,
                        principalTable: "LanguageStrings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopicArea_LanguageStringId",
                table: "TopicArea",
                column: "LanguageStringId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopicArea");

            migrationBuilder.DropColumn(
                name: "TopicAreaId",
                table: "LanguageStrings");
        }
    }
}
