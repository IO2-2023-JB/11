using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class UserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccountBalance",
                table: "AspNetUsers",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "AccountBalance", "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { 0m, "7169a59c-8b2e-4ff2-993f-5c329b2a6fc2", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AQAAAAIAAYagAAAAEEzyGjyVkSp5sE5gTxf4PXdS6Kt2cnKOKBWVwA9k4HFMXC/eCC4y4E2poVQiTJ75VQ==", "43370052-db22-4706-8023-7b2d4840b8ac" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EBD31DD-0321-4FDA-92FA-CD22A1190DC8",
                columns: new[] { "AccountBalance", "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { 0m, "e20dd8e7-62b9-4eda-9c13-60b07d79b630", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AQAAAAIAAYagAAAAEF0Gnim241fnVknmVI9f9O3TunQC4YKo5fGNi88FKRM9XhvBAWwzmqTqpgFaVcrozw==", "42134827-5567-428e-8e2a-c5f19aa3d341" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "CB6A6951-E91A-4A13-B6AC-8634883F5B93",
                columns: new[] { "AccountBalance", "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { 0m, "2a011fd1-6a12-4600-9084-9aa307cd0e16", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AQAAAAIAAYagAAAAEIEBhT4QAFjbHWsVx986wj/7BxLb3L00h/L8U3KqYmjXNSzgDlstMQ5I+R73qY3ajg==", "9d0f50d9-3cbe-478f-94b2-11733885d787" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBalance",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4a290514-864c-4578-b258-5610fb5ba15c", "AQAAAAIAAYagAAAAEJ1SadBj5+/H5lEMnl8xwvF3neCCjjK36ZMdMsl4m2pOCjmQQQloMeYSbGmXi1MnUw==", "f43d5208-1280-4cc5-bdb5-6ebeabd992ab" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EBD31DD-0321-4FDA-92FA-CD22A1190DC8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad757fe6-7487-41b4-b58e-3754ed1cacb8", "AQAAAAIAAYagAAAAEBClVNEsN4L/rYlhBLY6jhWcOuhErdSsvLr8yxlY91PBrLAbfoOOJZ8nICfCw0QNMw==", "6c71805d-b9b2-4718-9865-16de12544790" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "CB6A6951-E91A-4A13-B6AC-8634883F5B93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f2b70630-e776-437f-b14e-4edb18833145", "AQAAAAIAAYagAAAAEME1mU9yd3uviahPYwP0l8FMtdvLYY2K4t3lVZ1s7EjnmdLVX19kX2/jJdevnwzAdg==", "04252267-18d3-4208-845a-78e6efab4c48" });
        }
    }
}
