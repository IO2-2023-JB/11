using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class CommentsResponseToCommentResponses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentsResponse_AspNetUsers_AuthorId",
                table: "CommentsResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentsResponse_Comments_RespondOnId",
                table: "CommentsResponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentsResponse",
                table: "CommentsResponse");

            migrationBuilder.RenameTable(
                name: "CommentsResponse",
                newName: "CommentResponses");

            migrationBuilder.RenameIndex(
                name: "IX_CommentsResponse_RespondOnId",
                table: "CommentResponses",
                newName: "IX_CommentResponses_RespondOnId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentsResponse_AuthorId",
                table: "CommentResponses",
                newName: "IX_CommentResponses_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentResponses",
                table: "CommentResponses",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f0413015-5eb8-4867-a5d8-f5b173200b10", "AQAAAAIAAYagAAAAECcYtIxcbc9wQJNj4tpawC5pDg9+yfm5Elpcd7JPgXKscqtTVG8PbRtm5kobnHFiGA==", "60e795e4-2b20-4636-92be-03c0f187ebd0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EBD31DD-0321-4FDA-92FA-CD22A1190DC8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95a17589-e38d-4584-919a-9704b0f43a40", "AQAAAAIAAYagAAAAEF9kVmqzOIg9Ng9scPlsWwkmzfvl4LuuwDHfjXYU4isda1asJUAnXZ/+veQypsNL/g==", "050da823-7413-4e78-8af9-812279b1bfb7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "CB6A6951-E91A-4A13-B6AC-8634883F5B93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d51ab99-c4f7-49fe-8612-2d1b9c41705c", "AQAAAAIAAYagAAAAEBRRlhuTpYHUG+QKnbsMGPk26xwL9gtQ2yRHoC/Y4525agK7kI/Al4OHOf7+k6q5JQ==", "828a5da8-2d89-4d6b-a7ca-0e4adb51a246" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentResponses_AspNetUsers_AuthorId",
                table: "CommentResponses",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentResponses_Comments_RespondOnId",
                table: "CommentResponses",
                column: "RespondOnId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentResponses_AspNetUsers_AuthorId",
                table: "CommentResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentResponses_Comments_RespondOnId",
                table: "CommentResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentResponses",
                table: "CommentResponses");

            migrationBuilder.RenameTable(
                name: "CommentResponses",
                newName: "CommentsResponse");

            migrationBuilder.RenameIndex(
                name: "IX_CommentResponses_RespondOnId",
                table: "CommentsResponse",
                newName: "IX_CommentsResponse_RespondOnId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentResponses_AuthorId",
                table: "CommentsResponse",
                newName: "IX_CommentsResponse_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentsResponse",
                table: "CommentsResponse",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsResponse_AspNetUsers_AuthorId",
                table: "CommentsResponse",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsResponse_Comments_RespondOnId",
                table: "CommentsResponse",
                column: "RespondOnId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
