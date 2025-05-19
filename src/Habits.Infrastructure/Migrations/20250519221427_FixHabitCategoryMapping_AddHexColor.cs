using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Habbits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixHabitCategoryMapping_AddHexColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habits_HabitCategory_HabitCategoryId",
                table: "Habits");

            migrationBuilder.DropTable(
                name: "HabitCategory");

            migrationBuilder.CreateTable(
                name: "HabitCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: false),
                    HexColor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitCategories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_HabitCategories_HabitCategoryId",
                table: "Habits",
                column: "HabitCategoryId",
                principalTable: "HabitCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habits_HabitCategories_HabitCategoryId",
                table: "Habits");

            migrationBuilder.DropTable(
                name: "HabitCategories");

            migrationBuilder.CreateTable(
                name: "HabitCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitCategory", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_HabitCategory_HabitCategoryId",
                table: "Habits",
                column: "HabitCategoryId",
                principalTable: "HabitCategory",
                principalColumn: "Id");
        }
    }
}
