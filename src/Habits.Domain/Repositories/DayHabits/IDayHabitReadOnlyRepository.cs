namespace Habits.Domain.Repositories.DayHabits
{
    public interface IDayHabitReadOnlyRepository
    {
        Task<Dictionary<DateOnly, (int possible, int completed)>> GetDateRangeSummaryAsync(Guid userId, DateOnly startDate, DateOnly endDate);

        Task<List<(long habitId, string title, string? categoryName, bool isCompleted, DateTime createdAt, DateTime? updatedAt)>> GetHabitsForDateAsync(Guid userId, DateOnly date);
    }
}