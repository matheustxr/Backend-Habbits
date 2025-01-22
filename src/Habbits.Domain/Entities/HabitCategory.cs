namespace Habbits.Domain.Entities;

public class HabitCategory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<Habit> Habits { get; set; } = [];
}
