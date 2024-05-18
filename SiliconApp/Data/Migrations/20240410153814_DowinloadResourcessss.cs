using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiliconApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DowinloadResourcessss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DowinloadResource",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DowinloadResource",
                table: "Courses");
        }
    }
}
