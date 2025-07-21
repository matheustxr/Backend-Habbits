namespace Habits.Domain.Entities
{
    public class DayHabit
    {
        public long Id { get; set; }
        public long HabitId { get; set; }
        public Habit? Habit { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
