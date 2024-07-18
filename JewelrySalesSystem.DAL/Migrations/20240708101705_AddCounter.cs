using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CounterId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CounterId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InvoiceDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Counter",
                columns: table => new
                {
                    CounterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CounterName = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counter", x => x.CounterId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_CounterId",
                table: "User",
                column: "CounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CounterId",
                table: "Product",
                column: "CounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Counter_CounterId",
                table: "Product",
                column: "CounterId",
                principalTable: "Counter",
                principalColumn: "CounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Counter_CounterId",
                table: "User",
                column: "CounterId",
                principalTable: "Counter",
                principalColumn: "CounterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Counter_CounterId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Counter_CounterId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Counter");

            migrationBuilder.DropIndex(
                name: "IX_User_CounterId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Product_CounterId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CounterId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CounterId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InvoiceDetail");
        }
    }
}
