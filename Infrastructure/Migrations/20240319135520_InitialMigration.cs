using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModeratorOverviewStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeratorOverviewStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportBodyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportBodyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportMakes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportMakes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<byte[]>(type: "bytea", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportMakeModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportMakeId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransportModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportMakeModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportMakeModels_TransportMakes_TransportMakeId",
                        column: x => x.TransportMakeId,
                        principalTable: "TransportMakes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportMakeModels_TransportModels_TransportModelId",
                        column: x => x.TransportModelId,
                        principalTable: "TransportModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransportAdvertisements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EngineCapacity = table.Column<double>(type: "double precision", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    FuelConsumption = table.Column<double>(type: "double precision", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Mileage = table.Column<int>(type: "integer", nullable: false),
                    ManufactureCountry = table.Column<string>(type: "text", nullable: true),
                    ManufactureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsElectric = table.Column<bool>(type: "boolean", nullable: false),
                    IsNew = table.Column<bool>(type: "boolean", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    MakeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    BodyTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModeratorOverviewStatusId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    SubTitle = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportAdvertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportAdvertisements_ModeratorOverviewStatuses_Moderator~",
                        column: x => x.ModeratorOverviewStatusId,
                        principalTable: "ModeratorOverviewStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportAdvertisements_TransportBodyTypes_BodyTypeId",
                        column: x => x.BodyTypeId,
                        principalTable: "TransportBodyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportAdvertisements_TransportMakes_MakeId",
                        column: x => x.MakeId,
                        principalTable: "TransportMakes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportAdvertisements_TransportModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "TransportModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportAdvertisements_TransportTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TransportTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransportTypeBodyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransportBodyTypeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportTypeBodyTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportTypeBodyTypes_TransportBodyTypes_TransportBodyType~",
                        column: x => x.TransportBodyTypeId,
                        principalTable: "TransportBodyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportTypeBodyTypes_TransportTypes_TransportTypeId",
                        column: x => x.TransportTypeId,
                        principalTable: "TransportTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransportTypeMakes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransportMakeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportTypeMakes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportTypeMakes_TransportMakes_TransportMakeId",
                        column: x => x.TransportMakeId,
                        principalTable: "TransportMakes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportTypeMakes_TransportTypes_TransportTypeId",
                        column: x => x.TransportTypeId,
                        principalTable: "TransportTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransportAdvertisementImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportAdvertisementId = table.Column<Guid>(type: "uuid", nullable: true),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportAdvertisementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportAdvertisementImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportAdvertisementImages_TransportAdvertisements_Transp~",
                        column: x => x.TransportAdvertisementId,
                        principalTable: "TransportAdvertisements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisementImages_ImageId",
                table: "TransportAdvertisementImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisementImages_TransportAdvertisementId",
                table: "TransportAdvertisementImages",
                column: "TransportAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_BodyTypeId",
                table: "TransportAdvertisements",
                column: "BodyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_MakeId",
                table: "TransportAdvertisements",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_ModelId",
                table: "TransportAdvertisements",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_ModeratorOverviewStatusId",
                table: "TransportAdvertisements",
                column: "ModeratorOverviewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportAdvertisements_TypeId",
                table: "TransportAdvertisements",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportMakeModels_TransportMakeId",
                table: "TransportMakeModels",
                column: "TransportMakeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportMakeModels_TransportModelId",
                table: "TransportMakeModels",
                column: "TransportModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTypeBodyTypes_TransportBodyTypeId",
                table: "TransportTypeBodyTypes",
                column: "TransportBodyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTypeBodyTypes_TransportTypeId",
                table: "TransportTypeBodyTypes",
                column: "TransportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTypeMakes_TransportMakeId",
                table: "TransportTypeMakes",
                column: "TransportMakeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTypeMakes_TransportTypeId",
                table: "TransportTypeMakes",
                column: "TransportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportAdvertisementImages");

            migrationBuilder.DropTable(
                name: "TransportMakeModels");

            migrationBuilder.DropTable(
                name: "TransportTypeBodyTypes");

            migrationBuilder.DropTable(
                name: "TransportTypeMakes");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "TransportAdvertisements");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ModeratorOverviewStatuses");

            migrationBuilder.DropTable(
                name: "TransportBodyTypes");

            migrationBuilder.DropTable(
                name: "TransportMakes");

            migrationBuilder.DropTable(
                name: "TransportModels");

            migrationBuilder.DropTable(
                name: "TransportTypes");
        }
    }
}
