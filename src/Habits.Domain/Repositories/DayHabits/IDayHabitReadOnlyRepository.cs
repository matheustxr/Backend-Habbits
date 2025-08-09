namespace Habits.Domain.Repositories.DayHabits
{
    public interface IDayHabitReadOnlyRepository
    {
        Task<Dictionary<DateOnly, (int possible, int completed)>> GetMonthlySummaryAsync(Guid userId, DateOnly startDate, DateOnly endDate);

        Task<List<(long habitId, string title, string? categoryName, bool isCompleted)>> GetHabitsForDateAsync(Guid userId, DateOnly date);
    }
}