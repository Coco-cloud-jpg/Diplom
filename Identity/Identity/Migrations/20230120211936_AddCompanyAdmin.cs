using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class AddCompanyAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("37ea2e17-d3ef-4026-b9d1-697e374faaf2"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("00001457-f277-4ca9-9cf0-63d0f9034d93"), "CompanyAdmin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "RoleId" },
                values: new object[] { new Guid("b7f2e0c4-8d0b-4007-80a2-affd8f68135d"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00001457-f277-4ca9-9cf0-63d0f9034d93"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("b7f2e0c4-8d0b-4007-80a2-affd8f68135d"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "RoleId" },
                values: new object[] { new Guid("37ea2e17-d3ef-4026-b9d1-697e374faaf2"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }
    }
}
