using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBuyInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyInvoiceId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BuyInvoice",
                columns: table => new
                {
                    BuyInvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyInvoice", x => x.BuyInvoiceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_BuyInvoiceId",
                table: "OrderDetail",
                column: "BuyInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_BuyInvoice_BuyInvoiceId",
                table: "OrderDetail",
                column: "BuyInvoiceId",
                principalTable: "BuyInvoice",
                principalColumn: "BuyInvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_BuyInvoice_BuyInvoiceId",
                table: "OrderDetail");

            migrationBuilder.DropTable(
                name: "BuyInvoice");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_BuyInvoiceId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "BuyInvoiceId",
                table: "OrderDetail");
        }
    }
}
