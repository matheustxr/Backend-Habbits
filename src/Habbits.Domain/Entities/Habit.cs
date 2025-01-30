using Habbits.Domain.Enums;

namespace Habbits.Domain.Entities;

public class Habit
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    public List<WeekDays> WeekDays { get; set; } = new();
    public bool IsActive { get; set; } = true;

    public List<DayHabit> DayHabits { get; set; } = new();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public long? CategoryId { get; set; }
    public HabitCategory? Category { get; set; }
}
