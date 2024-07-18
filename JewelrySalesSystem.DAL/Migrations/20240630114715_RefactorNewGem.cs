using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelrySalesSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefactorNewGem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemPriceList_Gem_GemId",
                table: "GemPriceList");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Colour_ColourId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Colour");

            migrationBuilder.DropIndex(
                name: "IX_Product_ColourId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_GemPriceList_GemId",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "ColourId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CaratWeightPrice",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "ClarityPrice",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "ColourPrice",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "GemId",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "CaratWeight",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "Clarity",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "Colour",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "Cut",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Gem");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "User",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Product",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Invoice",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "CutPrice",
                table: "GemPriceList",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Gem",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Category",
                newName: "IsActive");

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "ProductMaterial",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Material",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CaratId",
                table: "GemPriceList",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClarityId",
                table: "GemPriceList",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "GemPriceList",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CutId",
                table: "GemPriceList",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffDate",
                table: "GemPriceList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "GemPriceList",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaratId",
                table: "Gem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClarityId",
                table: "Gem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Gem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CutId",
                table: "Gem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "Gem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShapeId",
                table: "Gem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Carat",
                columns: table => new
                {
                    CaratId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carat", x => x.CaratId);
                });

            migrationBuilder.CreateTable(
                name: "Clarity",
                columns: table => new
                {
                    ClarityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clarity", x => x.ClarityId);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    ColorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.ColorId);
                });

            migrationBuilder.CreateTable(
                name: "Cut",
                columns: table => new
                {
                    CutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cut", x => x.CutId);
                });

            migrationBuilder.CreateTable(
                name: "Origin",
                columns: table => new
                {
                    OriginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origin", x => x.OriginId);
                });

            migrationBuilder.CreateTable(
                name: "Shape",
                columns: table => new
                {
                    ShapeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceRate = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shape", x => x.ShapeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceList_CaratId",
                table: "GemPriceList",
                column: "CaratId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceList_ClarityId",
                table: "GemPriceList",
                column: "ClarityId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceList_ColorId",
                table: "GemPriceList",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceList_CutId",
                table: "GemPriceList",
                column: "CutId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceList_OriginId",
                table: "GemPriceList",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Gem_CaratId",
                table: "Gem",
                column: "CaratId");

            migrationBuilder.CreateIndex(
                name: "IX_Gem_ClarityId",
                table: "Gem",
                column: "ClarityId");

            migrationBuilder.CreateIndex(
                name: "IX_Gem_ColorId",
                table: "Gem",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Gem_CutId",
                table: "Gem",
                column: "CutId");

            migrationBuilder.CreateIndex(
                name: "IX_Gem_OriginId",
                table: "Gem",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Gem_ShapeId",
                table: "Gem",
                column: "ShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gem_Carat_CaratId",
                table: "Gem",
                column: "CaratId",
                principalTable: "Carat",
                principalColumn: "CaratId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gem_Clarity_ClarityId",
                table: "Gem",
                column: "ClarityId",
                principalTable: "Clarity",
                principalColumn: "ClarityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gem_Color_ColorId",
                table: "Gem",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gem_Cut_CutId",
                table: "Gem",
                column: "CutId",
                principalTable: "Cut",
                principalColumn: "CutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gem_Origin_OriginId",
                table: "Gem",
                column: "OriginId",
                principalTable: "Origin",
                principalColumn: "OriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gem_Shape_ShapeId",
                table: "Gem",
                column: "ShapeId",
                principalTable: "Shape",
                principalColumn: "ShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemPriceList_Carat_CaratId",
                table: "GemPriceList",
                column: "CaratId",
                principalTable: "Carat",
                principalColumn: "CaratId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemPriceList_Clarity_ClarityId",
                table: "GemPriceList",
                column: "ClarityId",
                principalTable: "Clarity",
                principalColumn: "ClarityId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemPriceList_Color_ColorId",
                table: "GemPriceList",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemPriceList_Cut_CutId",
                table: "GemPriceList",
                column: "CutId",
                principalTable: "Cut",
                principalColumn: "CutId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemPriceList_Origin_OriginId",
                table: "GemPriceList",
                column: "OriginId",
                principalTable: "Origin",
                principalColumn: "OriginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gem_Carat_CaratId",
                table: "Gem");

            migrationBuilder.DropForeignKey(
                name: "FK_Gem_Clarity_ClarityId",
                table: "Gem");

            migrationBuilder.DropForeignKey(
                name: "FK_Gem_Color_ColorId",
                table: "Gem");

            migrationBuilder.DropForeignKey(
                name: "FK_Gem_Cut_CutId",
                table: "Gem");

            migrationBuilder.DropForeignKey(
                name: "FK_Gem_Origin_OriginId",
                table: "Gem");

            migrationBuilder.DropForeignKey(
                name: "FK_Gem_Shape_ShapeId",
                table: "Gem");

            migrationBuilder.DropForeignKey(
                name: "FK_GemPriceList_Carat_CaratId",
                table: "GemPriceList");

            migrationBuilder.DropForeignKey(
                name: "FK_GemPriceList_Clarity_ClarityId",
                table: "GemPriceList");

            migrationBuilder.DropForeignKey(
                name: "FK_GemPriceList_Color_ColorId",
                table: "GemPriceList");

            migrationBuilder.DropForeignKey(
                name: "FK_GemPriceList_Cut_CutId",
                table: "GemPriceList");

            migrationBuilder.DropForeignKey(
                name: "FK_GemPriceList_Origin_OriginId",
                table: "GemPriceList");

            migrationBuilder.DropTable(
                name: "Carat");

            migrationBuilder.DropTable(
                name: "Clarity");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Cut");

            migrationBuilder.DropTable(
                name: "Origin");

            migrationBuilder.DropTable(
                name: "Shape");

            migrationBuilder.DropIndex(
                name: "IX_GemPriceList_CaratId",
                table: "GemPriceList");

            migrationBuilder.DropIndex(
                name: "IX_GemPriceList_ClarityId",
                table: "GemPriceList");

            migrationBuilder.DropIndex(
                name: "IX_GemPriceList_ColorId",
                table: "GemPriceList");

            migrationBuilder.DropIndex(
                name: "IX_GemPriceList_CutId",
                table: "GemPriceList");

            migrationBuilder.DropIndex(
                name: "IX_GemPriceList_OriginId",
                table: "GemPriceList");

            migrationBuilder.DropIndex(
                name: "IX_Gem_CaratId",
                table: "Gem");

            migrationBuilder.DropIndex(
                name: "IX_Gem_ClarityId",
                table: "Gem");

            migrationBuilder.DropIndex(
                name: "IX_Gem_ColorId",
                table: "Gem");

            migrationBuilder.DropIndex(
                name: "IX_Gem_CutId",
                table: "Gem");

            migrationBuilder.DropIndex(
                name: "IX_Gem_OriginId",
                table: "Gem");

            migrationBuilder.DropIndex(
                name: "IX_Gem_ShapeId",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ProductMaterial");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "CaratId",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "ClarityId",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "CutId",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "EffDate",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "GemPriceList");

            migrationBuilder.DropColumn(
                name: "CaratId",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "ClarityId",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "CutId",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Gem");

            migrationBuilder.DropColumn(
                name: "ShapeId",
                table: "Gem");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "User",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Product",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Invoice",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "GemPriceList",
                newName: "CutPrice");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Gem",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Category",
                newName: "Status");

            migrationBuilder.AddColumn<int>(
                name: "ColourId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CaratWeightPrice",
                table: "GemPriceList",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ClarityPrice",
                table: "GemPriceList",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ColourPrice",
                table: "GemPriceList",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "GemId",
                table: "GemPriceList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "CaratWeight",
                table: "Gem",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Clarity",
                table: "Gem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "Gem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cut",
                table: "Gem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "Gem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Colour",
                columns: table => new
                {
                    ColourId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColourName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colour", x => x.ColourId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ColourId",
                table: "Product",
                column: "ColourId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceList_GemId",
                table: "GemPriceList",
                column: "GemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GemPriceList_Gem_GemId",
                table: "GemPriceList",
                column: "GemId",
                principalTable: "Gem",
                principalColumn: "GemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Colour_ColourId",
                table: "Product",
                column: "ColourId",
                principalTable: "Colour",
                principalColumn: "ColourId");
        }
    }
}
