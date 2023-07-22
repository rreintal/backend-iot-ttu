using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class soaoaoaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicArea_LanguageStrings_LanguageStringId",
                table: "TopicArea");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicArea_TopicArea_ParentTopicAreaId",
                table: "TopicArea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicArea",
                table: "TopicArea");

            migrationBuilder.RenameTable(
                name: "TopicArea",
                newName: "TopicAreas");

            migrationBuilder.RenameIndex(
                name: "IX_TopicArea_ParentTopicAreaId",
                table: "TopicAreas",
                newName: "IX_TopicAreas_ParentTopicAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_TopicArea_LanguageStringId",
                table: "TopicAreas",
                newName: "IX_TopicAreas_LanguageStringId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicAreas",
                table: "TopicAreas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAreas_LanguageStrings_LanguageStringId",
                table: "TopicAreas",
                column: "LanguageStringId",
                principalTable: "LanguageStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAreas_TopicAreas_ParentTopicAreaId",
                table: "TopicAreas",
                column: "ParentTopicAreaId",
                principalTable: "TopicAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicAreas_LanguageStrings_LanguageStringId",
                table: "TopicAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAreas_TopicAreas_ParentTopicAreaId",
                table: "TopicAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicAreas",
                table: "TopicAreas");

            migrationBuilder.RenameTable(
                name: "TopicAreas",
                newName: "TopicArea");

            migrationBuilder.RenameIndex(
                name: "IX_TopicAreas_ParentTopicAreaId",
                table: "TopicArea",
                newName: "IX_TopicArea_ParentTopicAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_TopicAreas_LanguageStringId",
                table: "TopicArea",
                newName: "IX_TopicArea_LanguageStringId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicArea",
                table: "TopicArea",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicArea_LanguageStrings_LanguageStringId",
                table: "TopicArea",
                column: "LanguageStringId",
                principalTable: "LanguageStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicArea_TopicArea_ParentTopicAreaId",
                table: "TopicArea",
                column: "ParentTopicAreaId",
                principalTable: "TopicArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
