using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class AddedPackageTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("5e633f07-0b8d-40d3-a6b2-20f15cf09a0d"));

            migrationBuilder.CreateTable(
                name: "PackageTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxUsersCount = table.Column<int>(type: "int", nullable: false),
                    MaxRecordersCount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageTypeCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<short>(type: "smallint", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageTypeCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageTypeCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageTypeCompanies_PackageTypes_PackageTypeId",
                        column: x => x.PackageTypeId,
                        principalTable: "PackageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PackageTypes",
                columns: new[] { "Id", "Currency", "MaxRecordersCount", "MaxUsersCount", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("242964dd-6533-4af2-a42a-0544a3887ced"), (short)0, 15, 8, "Advanced USD", 50m },
                    { new Guid("6f1866d8-19b2-4a2c-9dd0-f331550eea5a"), (short)0, 25, 15, "High Capacity USD", 80m },
                    { new Guid("8dbaec20-a90a-43da-aaef-dbb89a5f598b"), (short)0, 5, 2, "Basic USD", 20m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "PasswordResetId", "RoleId" },
                values: new object[] { new Guid("90b00b93-5724-49a4-a26e-b40562eeb362"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", null, new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });

            migrationBuilder.CreateIndex(
                name: "IX_PackageTypeCompanies_CompanyId",
                table: "PackageTypeCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageTypeCompanies_PackageTypeId",
                table: "PackageTypeCompanies",
                column: "PackageTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageTypeCompanies");

            migrationBuilder.DropTable(
                name: "PackageTypes");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("90b00b93-5724-49a4-a26e-b40562eeb362"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "PasswordResetId", "RoleId" },
                values: new object[] { new Guid("5e633f07-0b8d-40d3-a6b2-20f15cf09a0d"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", null, new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }
    }
}
