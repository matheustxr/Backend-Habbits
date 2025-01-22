namespace Habbits.Domain.Entities;

public class DayHabit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HabitId { get; set; }
    public Habit Habit { get; set; }
    public DateTime Date { get; set; }
}
