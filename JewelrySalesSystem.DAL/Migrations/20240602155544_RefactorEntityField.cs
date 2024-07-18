using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefactorEntityField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstImage",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SecondImage",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "User",
                type: "nvarchar(12)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "FirstImage",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondImage",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
