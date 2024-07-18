using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EntityRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "BuyInvoice");

            migrationBuilder.RenameColumn(
                name: "SellPrice",
                table: "OrderDetail",
                newName: "PurchaseTotal");

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "PurchaseTotal",
                table: "OrderDetail",
                newName: "SellPrice");

            migrationBuilder.AddColumn<float>(
                name: "BuyPrice",
                table: "OrderDetail",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Total",
                table: "Order",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Total",
                table: "BuyInvoice",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
