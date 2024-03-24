using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class removeTAChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicAreas_TopicAreas_ParentTopicAreaId",
                table: "TopicAreas");

            migrationBuilder.DropIndex(
                name: "IX_TopicAreas_ParentTopicAreaId",
                table: "TopicAreas");

            migrationBuilder.DropColumn(
                name: "ParentTopicAreaId",
                table: "TopicAreas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentTopicAreaId",
                table: "TopicAreas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicAreas_ParentTopicAreaId",
                table: "TopicAreas",
                column: "ParentTopicAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAreas_TopicAreas_ParentTopicAreaId",
                table: "TopicAreas",
                column: "ParentTopicAreaId",
                principalTable: "TopicAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
