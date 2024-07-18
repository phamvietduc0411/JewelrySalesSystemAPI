using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EntityChanging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ProductMaterial");

            migrationBuilder.RenameColumn(
                name: "PurchaseTotal",
                table: "OrderDetail",
                newName: "Total");

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BuyInvoiceStatus",
                table: "BuyInvoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BuyInvoiceStatus",
                table: "BuyInvoice");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "OrderDetail",
                newName: "PurchaseTotal");

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "ProductMaterial",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
