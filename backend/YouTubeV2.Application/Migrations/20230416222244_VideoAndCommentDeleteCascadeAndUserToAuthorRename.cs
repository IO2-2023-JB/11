using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class VideoAndCommentDeleteCascadeAndUserToAuthorRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_UserId",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Videos",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_UserId",
                table: "Videos",
                newName: "IX_Videos_AuthorId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "511f563e-5faf-4580-9978-eb7fbf61aa34", "AQAAAAIAAYagAAAAEB01gZEFRP6fz14gMazmq6iGB16tkhYuloD81GpxGFa6KxEZs5h/TNP8c0E/F8dfmw==", "0ce542a9-4d18-49dc-b119-44e5f0c934c8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EBD31DD-0321-4FDA-92FA-CD22A1190DC8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3c27d07e-d2a9-4995-ae3d-ec08487d0d20", "AQAAAAIAAYagAAAAEITp6iTdAILkDuDWDKZOSMeOUO5IY67A7SYdfFwDdRYwhG5d2jUDHr2opzdJ07iZkQ==", "e7370a12-7cb3-4b4e-a0ae-a927f97ec086" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "CB6A6951-E91A-4A13-B6AC-8634883F5B93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8aba16f9-d7fe-4761-9a2e-db025916fb62", "AQAAAAIAAYagAAAAEL0fNnDOtfh0+t0h28Nf9yFVbypOpAaOGUA1AFLywHK/gRKhAVOYC2xi38p+urXakA==", "d05a0ce9-ff1c-4f6b-8ebf-c49c7bb56081" });

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AspNetUsers_AuthorId",
                table: "Videos",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_AuthorId",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Videos",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_AuthorId",
                table: "Videos",
                newName: "IX_Videos_UserId");

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
                name: "FK_Videos_AspNetUsers_UserId",
                table: "Videos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
