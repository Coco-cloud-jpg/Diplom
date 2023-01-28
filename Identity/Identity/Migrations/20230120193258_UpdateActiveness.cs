using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class UpdateActiveness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("c6e01b6d-7e08-43f3-9a21-b474e02c9e63"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "IsActive", "lastnName", "password", "RoleId" },
                values: new object[] { new Guid("a28e759d-a4a4-4447-ac7f-6826992567e0"), null, "palya1703@gmail.com", "Pavlo", false, "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: new Guid("a28e759d-a4a4-4447-ac7f-6826992567e0"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "CompanyId", "email", "firstName", "lastnName", "password", "RoleId" },
                values: new object[] { new Guid("c6e01b6d-7e08-43f3-9a21-b474e02c9e63"), null, "palya1703@gmail.com", "Pavlo", "Koval", "779498b489bd0915a7091d4bdfb95d0f2a1dfa8b4fd9003280b0c7984ffea817", new Guid("00551457-f277-4ca9-9cf0-611268bdd2a3") });
        }
    }
}
