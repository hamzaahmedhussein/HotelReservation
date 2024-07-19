using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelReservation.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93aa7f4a-4a56-4b85-9d99-5c42ebfea387");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a17ee1e7-4b18-47e8-8dd3-f745f2bb5f83");

            migrationBuilder.RenameColumn(
                name: "RoomType",
                table: "Rooms",
                newName: "BedsNumber");

            migrationBuilder.AddColumn<byte[]>(
                name: "RoomPicture",
                table: "Rooms",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d43e89c-4727-452b-8ac1-36c06d8eb79b", null, "Hotel", "HOTEL" },
                    { "a1d4223c-3786-4230-ad38-0d9ff8a006cc", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d43e89c-4727-452b-8ac1-36c06d8eb79b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1d4223c-3786-4230-ad38-0d9ff8a006cc");

            migrationBuilder.DropColumn(
                name: "RoomPicture",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "BedsNumber",
                table: "Rooms",
                newName: "RoomType");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "93aa7f4a-4a56-4b85-9d99-5c42ebfea387", null, "Hotel", "HOTEL" },
                    { "a17ee1e7-4b18-47e8-8dd3-f745f2bb5f83", null, "User", "USER" }
                });
        }
    }
}
