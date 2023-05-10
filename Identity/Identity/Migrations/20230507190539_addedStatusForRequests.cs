using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class addedStatusForRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IsActive",
                table: "PackageUpgradeRequests");

            migrationBuilder.AddColumn<short>(
                name: "Status",
                table: "PackageUpgradeRequests",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.InsertData(
                table: "PackageTypes",
                columns: new[] { "Id", "Currency", "MaxRecordersCount", "MaxUsersCount", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("284d0772-0835-43b0-9335-9ce2e670fe08"), (short)0, 5, 2, "Basic USD", 20m },
                    { new Guid("d87dc0a9-1cc9-4f6b-b85b-5e4bee55f4c7"), (short)0, 25, 15, "High Capacity USD", 80m },
                    { new Guid("e0e663e0-1bc7-42c6-8ffe-2b816bd15031"), (short)0, 15, 8, "Advanced USD", 50m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "PasswordResetId", "RoleId" },
                values: new object[] { new Guid("02184b2d-a3f2-4170-8c60-8ce23bb9bfbf"), null, "palya1703@gmail.com", "Pavlo", true, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", null, new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("284d0772-0835-43b0-9335-9ce2e670fe08"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("d87dc0a9-1cc9-4f6b-b85b-5e4bee55f4c7"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("e0e663e0-1bc7-42c6-8ffe-2b816bd15031"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("02184b2d-a3f2-4170-8c60-8ce23bb9bfbf"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PackageUpgradeRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PackageUpgradeRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
    }
}
