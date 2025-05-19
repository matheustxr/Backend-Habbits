namespace Habits.Domain.Entities;

public class HabitCategory
{
    public long Id { get; set; }
    public required string Category { get; set; }
    public string? HexColor { get; set; } = "#FFFFFF";
    public List<Habit>? Habits { get; set; } = [];
}
