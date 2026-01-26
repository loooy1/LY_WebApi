using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shirt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GuidId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Brand = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Color = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MyProperty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shirt", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "shirt",
                columns: new[] { "Id", "Brand", "Color", "Gender", "GuidId", "MyProperty", "Size" },
                values: new object[,]
                {
                    { 1, "品牌1", "黑", "男", new Guid("00000000-0000-0000-0000-000000000001"), 50, 5 },
                    { 2, "品牌2", "黑", "男", new Guid("00000000-0000-0000-0000-000000000002"), 51, 5 },
                    { 3, "品牌3", "黑", "男", new Guid("00000000-0000-0000-0000-000000000003"), 52, 5 },
                    { 4, "品牌4", "黑", "男", new Guid("00000000-0000-0000-0000-000000000004"), 53, 5 },
                    { 5, "品牌5", "黑", "男", new Guid("00000000-0000-0000-0000-000000000005"), 54, 5 },
                    { 6, "品牌6", "黑", "男", new Guid("00000000-0000-0000-0000-000000000006"), 55, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shirt");
        }
    }
}
