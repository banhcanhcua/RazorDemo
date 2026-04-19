using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RazorDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddNewProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Images", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 11, 1, "Mô tả sản phẩm 11", "[]", "Sản phẩm 11", 11000m, 100 },
                    { 12, 1, "Mô tả sản phẩm 12", "[]", "Sản phẩm 12", 12000m, 100 },
                    { 13, 1, "Mô tả sản phẩm 13", "[]", "Sản phẩm 13", 13000m, 100 },
                    { 14, 1, "Mô tả sản phẩm 14", "[]", "Sản phẩm 14", 14000m, 100 },
                    { 15, 1, "Mô tả sản phẩm 15", "[]", "Sản phẩm 15", 15000m, 100 },
                    { 16, 1, "Mô tả sản phẩm 16", "[]", "Sản phẩm 16", 16000m, 100 },
                    { 17, 1, "Mô tả sản phẩm 17", "[]", "Sản phẩm 17", 17000m, 100 },
                    { 18, 1, "Mô tả sản phẩm 18", "[]", "Sản phẩm 18", 18000m, 100 },
                    { 19, 1, "Mô tả sản phẩm 19", "[]", "Sản phẩm 19", 19000m, 100 },
                    { 20, 1, "Mô tả sản phẩm 20", "[]", "Sản phẩm 20", 20000m, 100 },
                    { 21, 1, "Mô tả sản phẩm 21", "[]", "Sản phẩm 21", 21000m, 100 },
                    { 22, 1, "Mô tả sản phẩm 22", "[]", "Sản phẩm 22", 22000m, 100 },
                    { 23, 1, "Mô tả sản phẩm 23", "[]", "Sản phẩm 23", 23000m, 100 },
                    { 24, 1, "Mô tả sản phẩm 24", "[]", "Sản phẩm 24", 24000m, 100 },
                    { 25, 1, "Mô tả sản phẩm 25", "[]", "Sản phẩm 25", 25000m, 100 },
                    { 26, 1, "Mô tả sản phẩm 26", "[]", "Sản phẩm 26", 26000m, 100 },
                    { 27, 1, "Mô tả sản phẩm 27", "[]", "Sản phẩm 27", 27000m, 100 },
                    { 28, 1, "Mô tả sản phẩm 28", "[]", "Sản phẩm 28", 28000m, 100 },
                    { 29, 1, "Mô tả sản phẩm 29", "[]", "Sản phẩm 29", 29000m, 100 },
                    { 30, 1, "Mô tả sản phẩm 30", "[]", "Sản phẩm 30", 30000m, 100 },
                    { 31, 1, "Mô tả sản phẩm 31", "[]", "Sản phẩm 31", 31000m, 100 },
                    { 32, 1, "Mô tả sản phẩm 32", "[]", "Sản phẩm 32", 32000m, 100 },
                    { 33, 1, "Mô tả sản phẩm 33", "[]", "Sản phẩm 33", 33000m, 100 },
                    { 34, 1, "Mô tả sản phẩm 34", "[]", "Sản phẩm 34", 34000m, 100 },
                    { 35, 1, "Mô tả sản phẩm 35", "[]", "Sản phẩm 35", 35000m, 100 },
                    { 36, 1, "Mô tả sản phẩm 36", "[]", "Sản phẩm 36", 36000m, 100 },
                    { 37, 1, "Mô tả sản phẩm 37", "[]", "Sản phẩm 37", 37000m, 100 },
                    { 38, 1, "Mô tả sản phẩm 38", "[]", "Sản phẩm 38", 38000m, 100 },
                    { 39, 1, "Mô tả sản phẩm 39", "[]", "Sản phẩm 39", 39000m, 100 },
                    { 40, 1, "Mô tả sản phẩm 40", "[]", "Sản phẩm 40", 40000m, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 40);
        }
    }
}
