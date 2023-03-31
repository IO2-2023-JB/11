using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class Subscription : Migration
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

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubcriberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriberId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubscribeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Subscriptions_AspNetUsers_SubscribeeId",
                        column: x => x.SubscribeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_AspNetUsers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1720390f-cce7-49d8-8b55-34a35ad4badb", null, "Simple", "SIMPLE" },
                    { "833cb83d-632b-4523-bebd-83131f458f9c", null, "Administrator", "ADMINISTRATOR" },
                    { "99a54b09-5058-4d2a-9ba7-01a8eae96b81", null, "Creator", "CREATOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscribeeId",
                table: "Subscriptions",
                column: "SubscribeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriberId",
                table: "Subscriptions",
                column: "SubscriberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1720390f-cce7-49d8-8b55-34a35ad4badb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "833cb83d-632b-4523-bebd-83131f458f9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99a54b09-5058-4d2a-9ba7-01a8eae96b81");

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
