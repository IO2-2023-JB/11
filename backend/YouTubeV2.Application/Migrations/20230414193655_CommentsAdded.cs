using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class CommentsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentsResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RespondOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentsResponse_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentsResponse_Comments_RespondOnId",
                        column: x => x.RespondOnId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b2e6228f-c1fc-4778-9822-e41bb765794b", "AQAAAAIAAYagAAAAEEDxDy5540/j+uJ1DQ6HpDzI0xNndaXHEeQYXidnc12bXmxZ/OVWYgO7yjU1vjJGUA==", "edc4d024-e8f2-4c6e-86d8-53624942a35c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EBD31DD-0321-4FDA-92FA-CD22A1190DC8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d56b95af-996c-40f2-b1e5-27c0a00f7c66", "AQAAAAIAAYagAAAAEHwrVfyyJntJHQKwSGhMMKBRBSa6mtlMhraq51UYKHNmFR95U52JJb8pzZ46mhf0xw==", "a58430a1-c365-42ad-b8d2-da11491c8a75" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "CB6A6951-E91A-4A13-B6AC-8634883F5B93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fdc72214-bb47-4a0f-b761-4b545e693c2b", "AQAAAAIAAYagAAAAEEMSXJH3YdbgqaPGCTS5Tr04cqVfp08Z/GuW4AK0pq4I8X6h7MYOvwW7MA3I7wcLtA==", "aa9e5147-97c3-4c04-a4b4-56a50f96501d" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_VideoId",
                table: "Comments",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsResponse_AuthorId",
                table: "CommentsResponse",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsResponse_RespondOnId",
                table: "CommentsResponse",
                column: "RespondOnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentsResponse");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8b502f63-4717-4180-a782-998d9ccd3c2b", "AQAAAAIAAYagAAAAENfL3ShmDUXWqUSJSbnzkc2RMhtdW+b0Hinx/rObc3A43PIOGx6RLGR8MT5IkPPquw==", "44d91bb0-39cd-4b82-a77b-67afd2942474" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EBD31DD-0321-4FDA-92FA-CD22A1190DC8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f39285f5-abae-414e-a484-a45cdf66188a", "AQAAAAIAAYagAAAAEA+L9ASrjRPxC4j3nSqjCXv1z7hqVNv+JX5rJJXW5Sm9EVvIrtjsSYPQrD1xBXpbvg==", "9bcb5a4b-38f9-4bf6-9730-fe09bccc11f0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "CB6A6951-E91A-4A13-B6AC-8634883F5B93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3eed7e77-5dbc-427d-898f-1f4184cebdde", "AQAAAAIAAYagAAAAEJiZvxw0R8PLyPTaYnIksvBzgxR0hW38Bk/2STl466hk9a1m1i/XhLlOPPP42YNDVA==", "ab902e25-0d69-4c46-9ce1-d8d72312ee52" });
        }
    }
}
