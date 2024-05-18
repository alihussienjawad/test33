using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiliconApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class isSavedIsLiked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_Liked",
                table: "UserSavedItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Saved",
                table: "UserSavedItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_Liked",
                table: "UserSavedItems");

            migrationBuilder.DropColumn(
                name: "Is_Saved",
                table: "UserSavedItems");
        }
    }
}
