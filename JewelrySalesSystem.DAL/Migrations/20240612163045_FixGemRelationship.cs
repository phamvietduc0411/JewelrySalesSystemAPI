using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixGemRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GemPriceList_GemId",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "EffDate",
                table: "GemPriceList");

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceList_GemId",
                table: "GemPriceList",
                column: "GemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GemPriceList_GemId",
                table: "GemPriceList");

            migrationBuilder.AddColumn<DateTime>(
                name: "EffDate",
                table: "GemPriceList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceList_GemId",
                table: "GemPriceList",
                column: "GemId");
        }
    }
}
