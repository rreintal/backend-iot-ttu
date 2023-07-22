using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class soaoaoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TopicAreaId",
                table: "TopicArea",
                newName: "ParentTopicAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicArea_ParentTopicAreaId",
                table: "TopicArea",
                column: "ParentTopicAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicArea_TopicArea_ParentTopicAreaId",
                table: "TopicArea",
                column: "ParentTopicAreaId",
                principalTable: "TopicArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicArea_TopicArea_ParentTopicAreaId",
                table: "TopicArea");

            migrationBuilder.DropIndex(
                name: "IX_TopicArea_ParentTopicAreaId",
                table: "TopicArea");

            migrationBuilder.RenameColumn(
                name: "ParentTopicAreaId",
                table: "TopicArea",
                newName: "TopicAreaId");
        }
    }
}
