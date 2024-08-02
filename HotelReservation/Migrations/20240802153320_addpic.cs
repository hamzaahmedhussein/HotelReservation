using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelReservation.Migrations
{
    /// <inheritdoc />
    public partial class addpic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26ecd7ff-95aa-4013-a00d-c6418c65d299");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "423fcaf3-aab6-44c4-9543-225ea0a1ff90");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1567de34-3b04-4e60-ac88-80e555d50e73", null, "User", "USER" },
                    { "bfbe4164-d81e-49f8-b2e0-d7732d0fbd3d", null, "Hotel", "HOTEL" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1567de34-3b04-4e60-ac88-80e555d50e73");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfbe4164-d81e-49f8-b2e0-d7732d0fbd3d");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Users");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26ecd7ff-95aa-4013-a00d-c6418c65d299", null, "Hotel", "HOTEL" },
                    { "423fcaf3-aab6-44c4-9543-225ea0a1ff90", null, "User", "USER" }
                });
        }
    }
}
