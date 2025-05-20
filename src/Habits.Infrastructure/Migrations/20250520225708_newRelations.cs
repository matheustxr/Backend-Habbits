using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Habbits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "HabitCategories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HabitCategories_UserId",
                table: "HabitCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitCategories_Users_UserId",
                table: "HabitCategories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitCategories_Users_UserId",
                table: "HabitCategories");

            migrationBuilder.DropIndex(
                name: "IX_HabitCategories_UserId",
                table: "HabitCategories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HabitCategories");
        }
    }
}
