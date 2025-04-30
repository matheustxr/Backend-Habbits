using Habits.Domain.Entities;
using Habits.Domain.Enums;
using Habits.Domain.Repositories.Habits;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure.DataAccess.Repositories;

public class HabitRepository : IHabitReadOnlyRepository, IHabitWriteOnlyRepository, IHabitUpdateOnlyRepository
{
    private readonly HabbitsDbContext _dbContext;

    public HabitRepository(HabbitsDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Habit habit)
    {
        await _dbContext.Habits.AddAsync(habit);
    }

    public async Task Delete(User user, long id)
    {
        var habitToRemove = await _dbContext.Habits
        .FirstOrDefaultAsync(h => h.Id == id && h.UserId == user.Id);

        if (habitToRemove is not null)
            _dbContext.Habits.Remove(habitToRemove);
    }

    public async Task<bool> ExistHabitWithTitle(string title)
    {
        return await _dbContext.Habits
            .AsNoTracking()
            .AnyAsync(h => h.Title == title);
    }

    public async Task<List<Habit>> GetAll(User user)
    {
        return await GetFullHabit()
            .AsNoTracking()
            .Where(habit => habit.UserId == user.Id).ToListAsync();
    }

    async Task<Habit?> IHabitReadOnlyRepository.GetById(User user, long id)
    {
        return await GetFullHabit()
            .AsNoTracking()
            .FirstOrDefaultAsync(habit => habit.Id == id && habit.UserId == user.Id);
    }

    async Task<Habit?> IHabitUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Habits
            .FirstOrDefaultAsync(habit => habit.Id == id && habit.UserId == user.Id);
    }

    public async Task<Habit?> GetHabitByTitle(string title)
    {
        return await _dbContext.Habits
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Title == title);
    }

    public async Task<List<Habit>> FilterByWeekday(User user, WeekDays weekday)
    {
        return await GetFullHabit()
            .AsNoTracking()
            .Where(h => h.UserId == user.Id && h.WeekDays!.Contains(weekday))
            .ToListAsync();
    }

    public void Update(Habit habit)
    {
        _dbContext.Habits.Update(habit);
    }

    private IQueryable<Habit> GetFullHabit()
    {
        return _dbContext.Habits
            .Include(h => h.WeekDays)
            .Include(h => h.DayHabits)
            .Include(h => h.HabitCategory);
    }
}
