using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdvertisementModel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportAdvertisements_TransportMakes_MakeId",
                table: "TransportAdvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportAdvertisements_TransportModels_ModelId",
                table: "TransportAdvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportAdvertisements_TransportTypes_TypeId",
                table: "TransportAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_TransportAdvertisements_MakeId",
                table: "TransportAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_TransportAdvertisements_ModelId",
                table: "TransportAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_TransportAdvertisements_TypeId",
                table: "TransportAdvertisements");

            migrationBuilder.DropColumn(
                name: "MakeId",
                table: "TransportAdvertisements");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "TransportAdvertisements");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "TransportAdvertisements");

            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "TransportAdvertisements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "TransportAdvertisements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "TransportAdvertisements",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Make",
                table: "TransportAdvertisements");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "TransportAdvertisements");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "TransportAdvertisements");

            migrationBuilder.AddColumn<Guid>(
                name: "MakeId",
                table: "TransportAdvertisements",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                table: "TransportAdvertisements",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "TransportAdvertisements",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_MakeId",
                table: "TransportAdvertisements",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_ModelId",
                table: "TransportAdvertisements",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_TypeId",
                table: "TransportAdvertisements",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportAdvertisements_TransportMakes_MakeId",
                table: "TransportAdvertisements",
                column: "MakeId",
                principalTable: "TransportMakes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportAdvertisements_TransportModels_ModelId",
                table: "TransportAdvertisements",
                column: "ModelId",
                principalTable: "TransportModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportAdvertisements_TransportTypes_TypeId",
                table: "TransportAdvertisements",
                column: "TypeId",
                principalTable: "TransportTypes",
                principalColumn: "Id");
        }
    }
}
