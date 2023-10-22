using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactBookApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7440164c-d480-4af1-8f71-85c857657cf1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9c2651e-9ea5-4dee-b330-ddfbfe40fce5");

            migrationBuilder.AddColumn<int>(
                name: "ContactCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9c58b723-424e-4ad0-aa88-89a787bcccc0", "2", "Regular", "Regular" },
                    { "d4cf84d0-f716-45a9-a903-f896e69a2444", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c58b723-424e-4ad0-aa88-89a787bcccc0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4cf84d0-f716-45a9-a903-f896e69a2444");

            migrationBuilder.DropColumn(
                name: "ContactCount",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7440164c-d480-4af1-8f71-85c857657cf1", "2", "Regular", "Regular" },
                    { "c9c2651e-9ea5-4dee-b330-ddfbfe40fce5", "1", "Admin", "Admin" }
                });
        }
    }
}
