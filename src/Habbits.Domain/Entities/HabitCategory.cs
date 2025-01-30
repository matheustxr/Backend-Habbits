namespace Habbits.Domain.Entities;

public class HabitCategory
{
    public long Id { get; set; }
    public required string Category { get; set; }
    public List<Habit>? Habits { get; set; } = [];
}
