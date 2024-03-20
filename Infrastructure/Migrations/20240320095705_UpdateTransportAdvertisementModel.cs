using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransportAdvertisementModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "TransportAdvertisements",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_CreatorId",
                table: "TransportAdvertisements",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportAdvertisements_Users_CreatorId",
                table: "TransportAdvertisements",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportAdvertisements_Users_CreatorId",
                table: "TransportAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_TransportAdvertisements_CreatorId",
                table: "TransportAdvertisements");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "TransportAdvertisements");
        }
    }
}
