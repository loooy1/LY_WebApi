using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "shirt",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "shirt");
        }
    }
}
