using System.Linq;
using Habits.Domain.Entities;
using Habits.Domain.Repositories.DayHabits;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure.DataAccess.Repositories
{
    public class DayHabitRepository : IDayHabitReadOnlyRepository, IDayHabitWriteOnlyRepository, IDayHabitUpdateOnlyRepository
    {
        private readonly HabitsDbContext _dbContext;

        public DayHabitRepository(HabitsDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(DayHabit dayHabit)
        {
            await _dbContext.DayHabits.AddAsync(dayHabit);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<(long habitId, string title, string? categoryName, bool isCompleted)>> GetHabitsForDateAsync(Guid userId, DateOnly date)
        {
            var dateStart = date.ToDateTime(TimeOnly.MinValue);

            var habits = await _dbContext.Habits
                .Include(h => h.HabitCategory)
                .Where(h => h.UserId == userId && h.IsActive && h.WeekDays != null && h.WeekDays.Contains((Domain.Enums.WeekDays)(int)date.DayOfWeek))
                .Select(h => new
                {
                    h.Id,
                    h.Title,
                    CategoryName = h.HabitCategory != null ? h.HabitCategory.Category : null,
                    IsCompleted = _dbContext.DayHabits.Any(dh => dh.HabitId == h.Id && dh.Date.Date == dateStart.Date && dh.IsCompleted)
                })
                .ToListAsync();

            return habits
                .Select(h => (h.Id, h.Title, h.CategoryName, h.IsCompleted))
                .ToList();
        }

        public async Task<Dictionary<DateOnly, (int possible, int completed)>> GetMonthlySummaryAsync(Guid userId, DateOnly startDate, DateOnly endDate)
        {
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = endDate.ToDateTime(TimeOnly.MaxValue);

            var days = await _dbContext.DayHabits
                .Where(dh => dh.Habit != null && dh.Habit.UserId == userId && dh.Date >= start && dh.Date <= end)
                .GroupBy(dh => dh.Date.Date)
                .Select(g => new
                {
                    Date = DateOnly.FromDateTime(g.Key),
                    Completed = g.Count(x => x.IsCompleted),
                    Possible = g.Select(x => x.HabitId).Distinct().Count()
                })
                .ToListAsync();

            return days.ToDictionary(
                d => d.Date,
                d => (d.Possible, d.Completed)
            );
        }

        public async Task ToggleCompletionStatusAsync(long habitId, DateOnly date)
        {
            var day = date.ToDateTime(TimeOnly.MinValue);

            var entry = await _dbContext.DayHabits
                .FirstOrDefaultAsync(dh => dh.HabitId == habitId && dh.Date.Date == day.Date);

            if (entry is null)
            {
                _dbContext.DayHabits.Add(new DayHabit
                {
                    HabitId = habitId,
                    Date = day,
                    IsCompleted = true
                });
            }
            else
            {
                entry.IsCompleted = !entry.IsCompleted;
                _dbContext.DayHabits.Update(entry);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}