namespace Habbits.Domain.Entities;

public class Habit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public List<int> WeekDays { get; set; } = [];

    public bool IsActive { get; set; } = true;
}
