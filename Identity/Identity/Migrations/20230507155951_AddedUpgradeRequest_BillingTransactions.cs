using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class AddedUpgradeRequest_BillingTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("242964dd-6533-4af2-a42a-0544a3887ced"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("6f1866d8-19b2-4a2c-9dd0-f331550eea5a"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("8dbaec20-a90a-43da-aaef-dbb89a5f598b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("90b00b93-5724-49a4-a26e-b40562eeb362"));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToPayForBills",
                table: "Companies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "BillingTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageUpgradeRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackagesCount = table.Column<int>(type: "int", nullable: false),
                    TimePosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageUpgradeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageUpgradeRequests_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageUpgradeRequests_PackageTypes_PackageTypeId",
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
                    { new Guid("3b93d9da-5479-4f01-9d1e-2a1c43fb966f"), (short)0, 25, 15, "High Capacity USD", 80m },
                    { new Guid("4c0ac95a-0762-4faf-9d89-696545ca2d78"), (short)0, 15, 8, "Advanced USD", 50m },
                    { new Guid("b288ffa4-a818-4e51-b594-4c9220f25048"), (short)0, 5, 2, "Basic USD", 20m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "PasswordResetId", "RoleId" },
                values: new object[] { new Guid("97a8240e-640c-4885-9b5d-06e90ac4be58"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", null, new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });

            migrationBuilder.CreateIndex(
                name: "IX_BillingTransactions_CompanyId",
                table: "BillingTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageUpgradeRequests_CompanyId",
                table: "PackageUpgradeRequests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageUpgradeRequests_PackageTypeId",
                table: "PackageUpgradeRequests",
                column: "PackageTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingTransactions");

            migrationBuilder.DropTable(
                name: "PackageUpgradeRequests");

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("3b93d9da-5479-4f01-9d1e-2a1c43fb966f"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("4c0ac95a-0762-4faf-9d89-696545ca2d78"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("b288ffa4-a818-4e51-b594-4c9220f25048"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("97a8240e-640c-4885-9b5d-06e90ac4be58"));

            migrationBuilder.DropColumn(
                name: "TimeToPayForBills",
                table: "Companies");

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
        }
    }
}
