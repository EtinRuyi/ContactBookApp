using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactBookApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveContactCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "27a8a7b2-0883-4ed9-b76d-db2c380e00f2", "2", "Regular", "Regular" },
                    { "44d70b48-566b-4eb7-87bf-256613948cda", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27a8a7b2-0883-4ed9-b76d-db2c380e00f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44d70b48-566b-4eb7-87bf-256613948cda");

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
    }
}
