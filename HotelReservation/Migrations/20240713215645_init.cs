using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelReservation.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "023823f9-4ef6-4385-abef-c65f99ad3cd3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e266f5f-fee1-44a8-9d94-5e0a9f577ee9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "93aa7f4a-4a56-4b85-9d99-5c42ebfea387", null, "Hotel", "HOTEL" },
                    { "a17ee1e7-4b18-47e8-8dd3-f745f2bb5f83", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93aa7f4a-4a56-4b85-9d99-5c42ebfea387");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a17ee1e7-4b18-47e8-8dd3-f745f2bb5f83");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "023823f9-4ef6-4385-abef-c65f99ad3cd3", null, "Hotel", "HOTEL" },
                    { "2e266f5f-fee1-44a8-9d94-5e0a9f577ee9", null, "User", "USER" }
                });
        }
    }
}
