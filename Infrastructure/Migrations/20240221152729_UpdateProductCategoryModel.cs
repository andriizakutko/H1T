using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_ProductCategories_ProductCategoryId",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypes_ProductCategoryId",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "ProductTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductCategoryId",
                table: "ProductTypes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_ProductCategoryId",
                table: "ProductTypes",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_ProductCategories_ProductCategoryId",
                table: "ProductTypes",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }
    }
}
