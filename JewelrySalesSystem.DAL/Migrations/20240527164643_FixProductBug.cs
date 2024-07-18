using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixProductBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warranty_Product_ProductId",
                table: "Warranty");

            migrationBuilder.DropIndex(
                name: "IX_Warranty_ProductId",
                table: "Warranty");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Warranty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Warranty",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_ProductId",
                table: "Warranty",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warranty_Product_ProductId",
                table: "Warranty",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId");
        }
    }
}
