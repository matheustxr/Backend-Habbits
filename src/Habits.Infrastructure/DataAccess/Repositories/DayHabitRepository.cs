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

        public async Task<List<(long habitId, string title, string? categoryName, bool isCompleted, DateTime createdAt, DateTime? updatedAt)>> GetHabitsForDateAsync(Guid userId, DateOnly date)
        {
            var dayOfWeek = (Domain.Enums.WeekDays)(int)date.DayOfWeek;

            var dateUtc = DateTime.SpecifyKind(date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

            var resultsFromDb = await _dbContext.Habits
                .AsNoTracking()
                .Where(h =>
                    h.UserId == userId &&
                    h.IsActive &&
                    h.CreatedAt.Date <= dateUtc &&
                    h.WeekDays != null &&
                    h.WeekDays.Contains(dayOfWeek))
                .Select(h => new 
                {
                    Id = h.Id,
                    Title = h.Title,
                    CategoryName = h.HabitCategory != null ? h.HabitCategory.Category : null,
                    IsCompleted = _dbContext.DayHabits.Any(dh => dh.HabitId == h.Id && dh.Date == date && dh.IsCompleted),
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt
                })
                .ToListAsync();

            return resultsFromDb
                .Select(result => (
                    result.Id,
                    result.Title,
                    result.CategoryName,
                    result.IsCompleted,
                    result.CreatedAt,
                    result.UpdatedAt
                ))
                .ToList();
        }

        public async Task<Dictionary<DateOnly, (int possible, int completed)>> GetDateRangeSummaryAsync(Guid userId, DateOnly startDate, DateOnly endDate)
        {
            var activeHabits = await _dbContext.Habits
                .AsNoTracking()
                .Where(h => h.UserId == userId && h.IsActive)
                .Select(h => new { h.CreatedAt, h.WeekDays })
                .ToListAsync();

            var completions = await _dbContext.DayHabits
                .AsNoTracking()
                .Where(dh => dh.Habit!.UserId == userId && dh.Date >= startDate && dh.Date <= endDate && dh.IsCompleted)
                .GroupBy(dh => dh.Date)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            var summary = new Dictionary<DateOnly, (int possible, int completed)>();

            for (var day = startDate; day <= endDate; day = day.AddDays(1))
            {
                var dayOfWeek = (Domain.Enums.WeekDays)(int)day.DayOfWeek;
                var dayUtc = DateTime.SpecifyKind(day.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

                var possibleCount = activeHabits
                    .Count(h =>
                        h.CreatedAt.Date <= dayUtc &&
                        h.WeekDays != null &&
                        h.WeekDays.Contains(dayOfWeek));

                var completedCount = completions.GetValueOrDefault(day, 0);

                if (possibleCount > 0 || completedCount > 0)
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
