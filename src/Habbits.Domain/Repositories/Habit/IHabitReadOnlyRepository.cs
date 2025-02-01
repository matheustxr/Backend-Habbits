namespace Habbits.Domain.Repositories.Habit;
public interface IHabitReadOnlyRepository
{
    Task<bool> ExistActiveHabitWithTitle(string habit);

    Task<Entities.Habit?> GetHabitByTitle(string habit);
}