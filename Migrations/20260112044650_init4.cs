using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test111",
                table: "shirt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test111",
                table: "shirt",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 1,
                column: "Test111",
                value: "");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 2,
                column: "Test111",
                value: "");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 3,
                column: "Test111",
                value: "");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 4,
                column: "Test111",
                value: "");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 5,
                column: "Test111",
                value: "");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 6,
                column: "Test111",
                value: "");
        }
    }
}
