using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Habits;
public interface IHabitReadOnlyRepository
{
    Task<List<Habit>> GetAll(User user);
    Task<bool> ExistActiveHabitWithTitle(string habit);
    Task<Habit?> GetHabitByTitle(string habit);
}