using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class AddCompanyDateCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("a28e759d-a4a4-4447-ac7f-6826992567e0"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Companies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "RoleId" },
                values: new object[] { new Guid("37ea2e17-d3ef-4026-b9d1-697e374faaf2"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("37ea2e17-d3ef-4026-b9d1-697e374faaf2"));

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Companies");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "RoleId" },
                values: new object[] { new Guid("a28e759d-a4a4-4447-ac7f-6826992567e0"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }
    }
}
