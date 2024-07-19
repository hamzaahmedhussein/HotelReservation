using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelReservation.Migrations
{
    /// <inheritdoc />
    public partial class hotelpic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d43e89c-4727-452b-8ac1-36c06d8eb79b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1d4223c-3786-4230-ad38-0d9ff8a006cc");

            migrationBuilder.AddColumn<byte[]>(
                name: "HotelPicture",
                table: "Hotels",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "969b7ec8-38b8-4638-bb2f-0d0511a9e48e", null, "User", "USER" },
                    { "fb07f8ba-8d30-47ed-890b-b72e61becd5d", null, "Hotel", "HOTEL" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "969b7ec8-38b8-4638-bb2f-0d0511a9e48e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb07f8ba-8d30-47ed-890b-b72e61becd5d");

            migrationBuilder.DropColumn(
                name: "HotelPicture",
                table: "Hotels");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d43e89c-4727-452b-8ac1-36c06d8eb79b", null, "Hotel", "HOTEL" },
                    { "a1d4223c-3786-4230-ad38-0d9ff8a006cc", null, "User", "USER" }
                });
        }
    }
}
