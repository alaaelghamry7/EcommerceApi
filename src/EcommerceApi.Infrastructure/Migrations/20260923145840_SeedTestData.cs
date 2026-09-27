using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Gadgets and devices", "Electronics" },
                    { 2, "Apparel and accessories", "Clothing" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Smartphone", 699.99m, 50 },
                    { 2, 1, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop", 1299.50m, 20 },
                    { 3, 2, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "T-Shirt", 19.99m, 200 },
                    { 4, 1, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Wireless Headphones", 149.99m, 75 },
                    { 5, 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Smartwatch", 249.99m, 40 },
                    { 6, 1, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Tablet", 499.00m, 30 },
                    { 7, 2, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Jeans", 49.99m, 150 },
                    { 8, 2, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Jacket", 89.99m, 60 },
                    { 9, 2, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Sneakers", 74.99m, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
