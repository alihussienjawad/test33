using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiliconApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Usersaved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedItem_AspNetUsers_ApplicationUserId",
                table: "UserSavedItem");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedItem_Courses_CourseId",
                table: "UserSavedItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSavedItem",
                table: "UserSavedItem");

            migrationBuilder.RenameTable(
                name: "UserSavedItem",
                newName: "UserSavedItems");

            migrationBuilder.RenameIndex(
                name: "IX_UserSavedItem_CourseId",
                table: "UserSavedItems",
                newName: "IX_UserSavedItems_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSavedItems",
                table: "UserSavedItems",
                columns: new[] { "ApplicationUserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedItems_AspNetUsers_ApplicationUserId",
                table: "UserSavedItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedItems_Courses_CourseId",
                table: "UserSavedItems",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedItems_AspNetUsers_ApplicationUserId",
                table: "UserSavedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedItems_Courses_CourseId",
                table: "UserSavedItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSavedItems",
                table: "UserSavedItems");

            migrationBuilder.RenameTable(
                name: "UserSavedItems",
                newName: "UserSavedItem");

            migrationBuilder.RenameIndex(
                name: "IX_UserSavedItems_CourseId",
                table: "UserSavedItem",
                newName: "IX_UserSavedItem_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSavedItem",
                table: "UserSavedItem",
                columns: new[] { "ApplicationUserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedItem_AspNetUsers_ApplicationUserId",
                table: "UserSavedItem",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedItem_Courses_CourseId",
                table: "UserSavedItem",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
