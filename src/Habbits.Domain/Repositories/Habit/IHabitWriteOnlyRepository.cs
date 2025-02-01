namespace Habbits.Domain.Repositories.Habit;
public interface IHabitWriteOnlyRepository
{
    Task Add(Entities.Habit habit);
    Task Delete(Entities.Habit habit);
}