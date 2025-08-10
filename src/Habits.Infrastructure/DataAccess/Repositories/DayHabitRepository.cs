using Habits.Domain.Entities;
using Habits.Domain.Repositories.DayHabits;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public async Task<Dictionary<DateOnly, (int possible, int completed)>> GetMonthlySummaryAsync(Guid userId, DateOnly startDate, DateOnly endDate)
        {
            var summary = await _dbContext.DayHabits
                .AsNoTracking()
                .Where(dh => dh.Habit != null && dh.Habit.UserId == userId && dh.Habit.IsActive && dh.Date >= startDate && dh.Date <= endDate)
                .GroupBy(dh => dh.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Possible = g.Select(x => x.HabitId).Distinct().Count(),
                    Completed = g.Count(x => x.IsCompleted)
                })
                .ToListAsync();

            return summary.ToDictionary(
                d => d.Date,
                d => (d.Possible, d.Completed)
            );
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
