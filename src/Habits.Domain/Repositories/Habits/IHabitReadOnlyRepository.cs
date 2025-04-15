namespace Habits.Domain.Repositories.Habits;
public interface IHabitReadOnlyRepository
{
    Task<bool> ExistActiveHabitWithTitle(string habit);

    Task<Entities.Habit?> GetHabitByTitle(string habit);
}