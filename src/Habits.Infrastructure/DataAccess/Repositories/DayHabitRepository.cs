using Habits.Domain.Entities;
using Habits.Domain.Repositories.DayHabits;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure.DataAccess.Repositories
{
    public class DayHabitRepository : IDayHabitReadOnlyRepository, IDayHabitWriteOnlyRepository
    {
        private readonly HabitsDbContext _dbContext;

        public DayHabitRepository(HabitsDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<(long habitId, string title, string? categoryName, bool isCompleted)>> GetHabitsForDateAsync(Guid userId, DateOnly date)
        {
            var possibleHabits = await _dbContext.Habits
                .Include(h => h.HabitCategory)
                .Where(h => h.UserId == userId && h.IsActive && h.WeekDays != null && h.WeekDays.Contains((Domain.Enums.WeekDays)(int)date.DayOfWeek))
                .ToListAsync();

            var completedHabitIds = (await _dbContext.DayHabits
                .Where(dh => dh.Habit != null && dh.Habit.UserId == userId && dh.Date == date && dh.IsCompleted)
                .Select(dh => dh.HabitId)
                .ToListAsync())
                .ToHashSet();

            return possibleHabits
                .Select(h => (
                    h.Id,
                    h.Title,
                    h.HabitCategory?.Category,
                    completedHabitIds.Contains(h.Id)
                ))
                .ToList();
        }

        public async Task<Dictionary<DateOnly, (int possible, int completed)>> GetDateRangeSummaryAsync(Guid userId, DateOnly startDate, DateOnly endDate)
        {
            var activeHabits = await _dbContext.Habits
                .AsNoTracking()
                .Where(h => h.UserId == userId && h.IsActive)
                .ToListAsync();

            var completions = await _dbContext.DayHabits
                .AsNoTracking()
                .Include(dh => dh.Habit) 
                .Where(dh => dh.Habit.UserId == userId && dh.Habit.IsActive && dh.Date >= startDate && dh.Date <= endDate && dh.IsCompleted)
                .GroupBy(dh => dh.Date)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            var summary = new Dictionary<DateOnly, (int possible, int completed)>();

            for (var day = startDate; day <= endDate; day = day.AddDays(1))
            {
                var possibleCount = activeHabits
                    .Count(h => h.WeekDays != null && h.WeekDays.Contains((Domain.Enums.WeekDays)(int)day.DayOfWeek));

                var completedCount = completions.GetValueOrDefault(day, 0);

                if (possibleCount > 0)
                {
                    summary[day] = (possibleCount, completedCount);
                }
            }

            return summary;
        }

        public async Task ToggleCompletionStatusAsync(long habitId, DateOnly date)
        {
            var entry = await _dbContext.DayHabits
                .FirstOrDefaultAsync(dh => dh.HabitId == habitId && dh.Date == date);

            if (entry is null)
            {
                _dbContext.DayHabits.Add(new DayHabit
                {
                    HabitId = habitId,
                    Date = date,
                    IsCompleted = true
                });
            }
            else
            {
                entry.IsCompleted = !entry.IsCompleted;
            }
        }
    }
}
