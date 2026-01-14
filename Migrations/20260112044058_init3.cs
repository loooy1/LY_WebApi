using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Test",
                table: "shirt",
                newName: "Test111");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Test111",
                table: "shirt",
                newName: "Test");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 1,
                column: "Test",
                value: "Test");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 2,
                column: "Test",
                value: "Test");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 3,
                column: "Test",
                value: "Test");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 4,
                column: "Test",
                value: "Test");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 5,
                column: "Test",
                value: "Test");

            migrationBuilder.UpdateData(
                table: "shirt",
                keyColumn: "Id",
                keyValue: 6,
                column: "Test",
                value: "Test");
        }
    }
}
