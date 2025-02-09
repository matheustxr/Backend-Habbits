using Habbits.Domain.Entities;
using Habbits.Domain.Repositories.Habit;
using Microsoft.EntityFrameworkCore;

namespace Habbits.Infrastructure.DataAccess.Repositories;

public class HabitRepository : IHabitReadOnlyRepository, IHabitWriteOnlyRepository, IHabitUpdateOnlyRepository
{
    private readonly HabbitsDbContext _dbContext;

    public HabitRepository(HabbitsDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Habit habit)
    {
        await _dbContext.Habits.AddAsync(habit);
    }

    public async Task Delete(Habit habit)
    {
        var habitToRemove = await _dbContext.Habits.FindAsync(habit.Id);
        _dbContext.Habits.Remove(habitToRemove!);
    }

    public async Task<bool> ExistActiveHabitWithTitle(string title)     
    {
        return await _dbContext.Habits.AnyAsync(h => h.Title == title);
    }

    public async Task<Habit> GetById(long id)
    {
        return await _dbContext.Habits.FirstAsync(habit => habit.Id == id);
    }

    public async Task<Habit?> GetHabitByTitle(string title)
    {
        return await _dbContext.Habits.AsNoTracking().FirstOrDefaultAsync(h => h.Title == title);
    }

    public void Update(Habit habit)
    {
        _dbContext.Habits.Update(habit);
    }
}
