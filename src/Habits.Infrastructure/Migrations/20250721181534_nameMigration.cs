using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Habbits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayHabit_Habits_HabitId",
                table: "DayHabit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DayHabit",
                table: "DayHabit");

            migrationBuilder.RenameTable(
                name: "DayHabit",
                newName: "DayHabits");

            migrationBuilder.RenameIndex(
                name: "IX_DayHabit_HabitId",
                table: "DayHabits",
                newName: "IX_DayHabits_HabitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DayHabits",
                table: "DayHabits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DayHabits_Habits_HabitId",
                table: "DayHabits",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayHabits_Habits_HabitId",
                table: "DayHabits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DayHabits",
                table: "DayHabits");

            migrationBuilder.RenameTable(
                name: "DayHabits",
                newName: "DayHabit");

            migrationBuilder.RenameIndex(
                name: "IX_DayHabits_HabitId",
                table: "DayHabit",
                newName: "IX_DayHabit_HabitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DayHabit",
                table: "DayHabit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DayHabit_Habits_HabitId",
                table: "DayHabit",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
