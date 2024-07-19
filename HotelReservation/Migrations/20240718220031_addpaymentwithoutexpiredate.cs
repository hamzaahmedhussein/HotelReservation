using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelReservation.Migrations
{
    /// <inheritdoc />
    public partial class addpaymentwithoutexpiredate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d7bf3e4-05b6-4de1-916b-5457ce8c25da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3cad0b2-a013-4aa4-b684-d4204d9e1cb6");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Payments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26ecd7ff-95aa-4013-a00d-c6418c65d299", null, "Hotel", "HOTEL" },
                    { "423fcaf3-aab6-44c4-9543-225ea0a1ff90", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26ecd7ff-95aa-4013-a00d-c6418c65d299");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "423fcaf3-aab6-44c4-9543-225ea0a1ff90");

            migrationBuilder.AddColumn<string>(
                name: "ExpirationDate",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d7bf3e4-05b6-4de1-916b-5457ce8c25da", null, "User", "USER" },
                    { "b3cad0b2-a013-4aa4-b684-d4204d9e1cb6", null, "Hotel", "HOTEL" }
                });
        }
    }
}
