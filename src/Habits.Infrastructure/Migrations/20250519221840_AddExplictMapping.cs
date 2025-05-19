using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Habbits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExplictMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habits_HabitCategories_HabitCategoryId",
                table: "Habits");

            migrationBuilder.DropIndex(
                name: "IX_Habits_HabitCategoryId",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "HabitCategoryId",
                table: "Habits");

            migrationBuilder.CreateIndex(
                name: "IX_Habits_CategoryId",
                table: "Habits",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_HabitCategories_CategoryId",
                table: "Habits",
                column: "CategoryId",
                principalTable: "HabitCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habits_HabitCategories_CategoryId",
                table: "Habits");

            migrationBuilder.DropIndex(
                name: "IX_Habits_CategoryId",
                table: "Habits");

            migrationBuilder.AddColumn<long>(
                name: "HabitCategoryId",
                table: "Habits",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Habits_HabitCategoryId",
                table: "Habits",
                column: "HabitCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_HabitCategories_HabitCategoryId",
                table: "Habits",
                column: "HabitCategoryId",
                principalTable: "HabitCategories",
                principalColumn: "Id");
        }
    }
}
