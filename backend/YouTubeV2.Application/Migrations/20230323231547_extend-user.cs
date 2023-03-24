using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class extenduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44b04ab7-e63c-4979-99eb-c8bef14630d0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51b0c472-5583-49e2-882e-f21e9234381e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f70541fd-4c68-4490-a6b8-7c616c1cd368");

            migrationBuilder.AddColumn<decimal>(
                name: "AccountBalance",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionsCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "AccountBalance",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionsCount",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44b04ab7-e63c-4979-99eb-c8bef14630d0", null, "Simple", "SIMPLE" },
                    { "51b0c472-5583-49e2-882e-f21e9234381e", null, "Administrator", "ADMINISTRATOR" },
                    { "f70541fd-4c68-4490-a6b8-7c616c1cd368", null, "Creator", "CREATOR" }
                });
        }
    }
}
