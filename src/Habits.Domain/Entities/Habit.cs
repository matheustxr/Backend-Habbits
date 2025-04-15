using Habits.Domain.Enums;

namespace Habits.Domain.Entities;

public class Habit
{
    public long Id { get; set; }
    public required string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    public List<WeekDays>? WeekDays { get; set; }
    public bool IsActive { get; set; } = true;

    public List<DayHabit>? DayHabits { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public long? CategoryId { get; set; }
    public HabitCategory? HabitCategory { get; set; }
}
