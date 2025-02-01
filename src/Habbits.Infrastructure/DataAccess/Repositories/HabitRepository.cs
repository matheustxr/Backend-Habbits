using Habbits.Domain.Entities;
using Habbits.Domain.Repositories.Habit;

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

    public void Update(Habit habit)
    {
        _dbContext.Habits.Update(habit);
    }
}
