using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class aoaoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HasTopicAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicAreaId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HasTopicAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HasTopicAreas_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HasTopicAreas_TopicAreas_TopicAreaId",
                        column: x => x.TopicAreaId,
                        principalTable: "TopicAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HasTopicAreas_NewsId",
                table: "HasTopicAreas",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_HasTopicAreas_TopicAreaId",
                table: "HasTopicAreas",
                column: "TopicAreaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HasTopicAreas");
        }
    }
}
