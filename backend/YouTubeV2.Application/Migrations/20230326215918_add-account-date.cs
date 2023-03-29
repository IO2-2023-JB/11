using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class addaccountdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16215aed-1161-4c85-8306-0a529f50d7a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82d4176c-49c6-41ee-b9bf-2318d64e94f1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2b1e341-c450-4d0f-80f8-af082960e2ec");

            migrationBuilder.AddColumn<DateTime>(
                name: "AccountCreationDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11652356-d320-40da-8fb9-96f8c874d8f2", null, "Simple", "SIMPLE" },
                    { "2db708d3-fd89-4f97-b156-29e35344295d", null, "Administrator", "ADMINISTRATOR" },
                    { "5b5909ef-e020-4af8-b3f5-a2c38db0e35a", null, "Creator", "CREATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11652356-d320-40da-8fb9-96f8c874d8f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2db708d3-fd89-4f97-b156-29e35344295d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b5909ef-e020-4af8-b3f5-a2c38db0e35a");

            migrationBuilder.DropColumn(
                name: "AccountCreationDate",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16215aed-1161-4c85-8306-0a529f50d7a3", null, "Creator", "CREATOR" },
                    { "82d4176c-49c6-41ee-b9bf-2318d64e94f1", null, "Administrator", "ADMINISTRATOR" },
                    { "c2b1e341-c450-4d0f-80f8-af082960e2ec", null, "Simple", "SIMPLE" }
                });
        }
    }
}
