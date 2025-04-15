namespace Habits.Domain.Repositories.Habits;
public interface IHabitWriteOnlyRepository
{
    Task Add(Entities.Habit habit);
    Task Delete(Entities.Habit habit);
}