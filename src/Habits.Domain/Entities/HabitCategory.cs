namespace Habits.Domain.Entities;

public class HabitCategory
{
    public long Id { get; set; }
    public required string Category { get; set; }
    public string? HexColor { get; set; } = "#FFFFFF";
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<Habit>? Habits { get; set; } = [];
}
