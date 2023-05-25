using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouTubeV2.Application.Migrations
{
    /// <inheritdoc />
    public partial class PlaylistCreationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "Playlists",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a73b225e-c92b-47ec-9f80-102b74caf9e8", "AQAAAAIAAYagAAAAEGpAm3Qfr1u0vZQYnhHIHb755hRKZUJzvzPzbplLP0TLEYO2Ik68q4JSmnjT/vfRvg==", "f9b1b064-c2b3-4e37-ab03-c7689c495025" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EBD31DD-0321-4FDA-92FA-CD22A1190DC8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "efcd6748-ca6c-4a83-b211-1c089f813e94", "AQAAAAIAAYagAAAAEOWiEEBA6KO4qNNG86ULtCptNNv2N6KTeJDcoQOE7KLpS2fAHmi5/PFJkaI6cqmc/Q==", "8379849d-ae5a-4b18-b6d5-5a9b305542e1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "CB6A6951-E91A-4A13-B6AC-8634883F5B93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1679c2c0-2e14-43b7-bc91-1d6445503ad7", "AQAAAAIAAYagAAAAEOHlZCG9k24WVMusgfAF5KDnsAEkKASg/kv/uAAWdyrnNT5T6bu+jq2d0sMiDgEz6g==", "46727137-38a6-4b0a-87f8-ddf906e9690a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Playlists");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0a68b9e-243b-49cf-b33b-7b946eca71db", "AQAAAAIAAYagAAAAEN8lpLfxrRkY3X3x+OAjkbsEN7hfQR1X0v5t2fuQNSaIslnfgu980CyYa0pHDsuPTw==", "973ab52b-55fc-45c2-80fa-e54107d588af" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6EBD31DD-0321-4FDA-92FA-CD22A1190DC8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "68b288dd-58aa-4c4a-8f9f-27110e97451c", "AQAAAAIAAYagAAAAECAPE+K/CoSs7Jf0SUrg359awex4pVLKw4oun9itkSJwrcJ5uHdx6IHZtujxlZ1F5g==", "5fd85a84-97c7-434c-806b-9c22f5e3e4cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "CB6A6951-E91A-4A13-B6AC-8634883F5B93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d85119b4-a417-46f5-950d-6cea1873f9c6", "AQAAAAIAAYagAAAAEJpRJOUuUIpKr4RTXQ/KF8LvfLMP1o6yv32pTdmKK4mta5sgv+05pFcoJEKa2d2wLw==", "9e83c101-7a02-47a9-8918-a5638bc1ee4a" });
        }
    }
}
