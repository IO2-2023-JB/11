using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class usermoneyandDateTimeOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AccountCreationDate",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountBalance",
                table: "AspNetUsers",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "183dc7b8-3978-48b8-b420-8f0dfc0f486c", null, "Simple", "SIMPLE" },
                    { "5f3d2916-2f43-474a-9c52-efd0f42f806e", null, "Administrator", "ADMINISTRATOR" },
                    { "cc9316a6-c840-4333-9e02-0288eea40665", null, "Creator", "CREATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "183dc7b8-3978-48b8-b420-8f0dfc0f486c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f3d2916-2f43-474a-9c52-efd0f42f806e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc9316a6-c840-4333-9e02-0288eea40665");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccountCreationDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountBalance",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

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
    }
}
