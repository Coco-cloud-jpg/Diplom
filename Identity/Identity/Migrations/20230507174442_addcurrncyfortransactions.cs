using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class addcurrncyfortransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<short>(
                name: "Currency",
                table: "BillingTransactions",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.InsertData(
                table: "PackageTypes",
                columns: new[] { "Id", "Currency", "MaxRecordersCount", "MaxUsersCount", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("4882b998-97eb-4744-ac2f-20a4899e55be"), (short)0, 5, 2, "Basic USD", 20m },
                    { new Guid("4e167b78-273c-496b-9339-0ec9072713de"), (short)0, 25, 15, "High Capacity USD", 80m },
                    { new Guid("a9ffb93c-34f2-44c5-84b8-662586b71709"), (short)0, 15, 8, "Advanced USD", 50m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "PasswordResetId", "RoleId" },
                values: new object[] { new Guid("d2f1beb4-425d-49df-aac2-b5ff3cb3d972"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", null, new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("4882b998-97eb-4744-ac2f-20a4899e55be"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("4e167b78-273c-496b-9339-0ec9072713de"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("a9ffb93c-34f2-44c5-84b8-662586b71709"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("d2f1beb4-425d-49df-aac2-b5ff3cb3d972"));

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "BillingTransactions");

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
        }
    }
}
