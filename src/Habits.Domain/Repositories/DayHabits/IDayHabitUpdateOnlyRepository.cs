namespace Habits.Domain.Repositories.DayHabits
{
    public interface IDayHabitUpdateOnlyRepository
    {
        Task ToggleCompletionStatusAsync(long habitId, DateOnly date);
    }
}
