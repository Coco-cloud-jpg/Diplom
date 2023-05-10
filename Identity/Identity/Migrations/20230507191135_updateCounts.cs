using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class updateCounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<short>(
                name: "PackagesCount",
                table: "PackageUpgradeRequests",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "PackageTypes",
                columns: new[] { "Id", "Currency", "MaxRecordersCount", "MaxUsersCount", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("008a13c8-0b08-43a1-8b86-bc2291aa5130"), (short)0, 5, 2, "Basic USD", 20m },
                    { new Guid("bc5ecb2b-6300-4d95-b2b2-97551c4cfe05"), (short)0, 25, 15, "High Capacity USD", 80m },
                    { new Guid("ea34b98f-e623-4162-8aa7-48538b57173a"), (short)0, 15, 8, "Advanced USD", 50m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "PasswordResetId", "RoleId" },
                values: new object[] { new Guid("4bb728ef-981b-4af5-b887-a09b9af2160b"), null, "palya1703@gmail.com", "Pavlo", true, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", null, new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("008a13c8-0b08-43a1-8b86-bc2291aa5130"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("bc5ecb2b-6300-4d95-b2b2-97551c4cfe05"));

            migrationBuilder.DeleteData(
                table: "PackageTypes",
                keyColumn: "Id",
                keyValue: new Guid("ea34b98f-e623-4162-8aa7-48538b57173a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("4bb728ef-981b-4af5-b887-a09b9af2160b"));

            migrationBuilder.AlterColumn<int>(
                name: "PackagesCount",
                table: "PackageUpgradeRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

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
    }
}
