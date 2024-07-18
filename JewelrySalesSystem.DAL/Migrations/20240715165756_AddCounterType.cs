using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCounterType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CounterTypeId",
                table: "Counter",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CounterType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CounterTypeName = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterType", x => x.Id);
                });           

            migrationBuilder.CreateIndex(
                name: "IX_Counter_CounterTypeId",
                table: "Counter",
                column: "CounterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counter_CounterType_CounterTypeId",
                table: "Counter",
                column: "CounterTypeId",
                principalTable: "CounterType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counter_CounterType_CounterTypeId",
                table: "Counter");

            migrationBuilder.DropTable(
                name: "CounterType");

            migrationBuilder.DropIndex(
                name: "IX_User_CounterId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Counter_CounterTypeId",
                table: "Counter");

            migrationBuilder.DropColumn(
                name: "CounterTypeId",
                table: "Counter");

            migrationBuilder.CreateIndex(
                name: "IX_User_CounterId",
                table: "User",
                column: "CounterId");
        }
    }
}
