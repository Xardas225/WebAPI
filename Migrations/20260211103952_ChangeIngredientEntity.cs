using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIngredientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ingredients");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ingredients",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "ingredients",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ingredients");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "ingredients");

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
