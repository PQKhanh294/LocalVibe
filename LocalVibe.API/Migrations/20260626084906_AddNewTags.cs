using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LocalVibe.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "Latitude", "Longitude", "Tag", "Title" },
                values: new object[,]
                {
                    { 39, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 20.262599999999999, 105.8613, "Hotel", "Homestay Bái Đính Ninh Bình" },
                    { 40, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 16.054400000000001, 108.2022, "Hotel", "Khách sạn Mường Thanh Đà Nẵng" },
                    { 41, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 12.238799999999999, 109.19670000000001, "Hotel", "Resort Ana Mandara Nha Trang" },
                    { 42, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 21.055800000000001, 107.76300000000001, "Hotel", "Homestay Cô Tô Quảng Ninh" },
                    { 43, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 15.880100000000001, 108.33799999999999, "Attraction", "Phố cổ Hội An" },
                    { 44, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 20.9101, 107.18389999999999, "Attraction", "Vịnh Hạ Long" },
                    { 45, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 16.469799999999999, 107.5784, "Attraction", "Hoàng Thành Huế" },
                    { 46, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 15.7644, 108.1169, "Attraction", "Thánh địa Mỹ Sơn" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 46);
        }
    }
}
