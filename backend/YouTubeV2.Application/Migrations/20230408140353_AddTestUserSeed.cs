using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddTestUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24de6e76-9a75-490b-bddc-a66e91e20fa4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37f0c423-8d1c-4817-a1af-e1bf60c9a0eb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87978f1a-4281-4a59-8ee8-52b1ed4b3d56");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39cc2fe2-d00d-4f48-a49d-005d8e983c72", null, "Simple", "SIMPLE" },
                    { "63798117-72aa-4bc5-a1ef-4e771204d561", null, "Creator", "CREATOR" },
                    { "b3a48a48-1a74-45da-a179-03b298bc53bc", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[] { "02174cf0–9412–4cfe-afbf-59f706d72cf6", 0, "deeb3074-4431-4eb8-8fd9-e6284ed1e234", "test@mail.com", false, false, null, "Prime", "TEST@MAIL.COM", "EXAMPLE", "AQAAAAIAAYagAAAAECKy912+seLJXiYrY/JhIiTJCjRmTAcoKgfPi3VLU1p7ih8g8MeDT+fnDSKeHXX9yA==", null, false, "1192c567-fd16-46cd-9270-78a4ce68e8c6", "Test", false, "Example" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "39cc2fe2-d00d-4f48-a49d-005d8e983c72", "02174cf0–9412–4cfe-afbf-59f706d72cf6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63798117-72aa-4bc5-a1ef-4e771204d561");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3a48a48-1a74-45da-a179-03b298bc53bc");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39cc2fe2-d00d-4f48-a49d-005d8e983c72", "02174cf0–9412–4cfe-afbf-59f706d72cf6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39cc2fe2-d00d-4f48-a49d-005d8e983c72");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24de6e76-9a75-490b-bddc-a66e91e20fa4", null, "Administrator", "ADMINISTRATOR" },
                    { "37f0c423-8d1c-4817-a1af-e1bf60c9a0eb", null, "Simple", "SIMPLE" },
                    { "87978f1a-4281-4a59-8ee8-52b1ed4b3d56", null, "Creator", "CREATOR" }
                });
        }
    }
}
