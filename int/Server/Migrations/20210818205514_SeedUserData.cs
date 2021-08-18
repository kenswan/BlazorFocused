using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Integration.Server.Migrations
{
    public partial class SeedUserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("16429cae-972c-4236-a7ab-704a8b01408e"), "94c412c7-21bd-4feb-b3cd-e14bf5c85b42", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("0288bf3f-2e1d-4084-ab7d-214dd0fdb118"), 0, "94e90695-4ce3-4a01-b1a2-65c169c4c257", "test@test.com", false, "John", "Doe", false, null, null, "INTUSERADMIN", "AQAAAAEAACcQAAAAEGujX9MQTa0jqT1pjdaIV5A++OUYvzFC8SN7edn4D2w57oUReG6YcLUm0GshAxhVhQ==", null, false, null, false, "IntUserAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("16429cae-972c-4236-a7ab-704a8b01408e"), new Guid("0288bf3f-2e1d-4084-ab7d-214dd0fdb118") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("16429cae-972c-4236-a7ab-704a8b01408e"), new Guid("0288bf3f-2e1d-4084-ab7d-214dd0fdb118") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("16429cae-972c-4236-a7ab-704a8b01408e"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0288bf3f-2e1d-4084-ab7d-214dd0fdb118"));
        }
    }
}
