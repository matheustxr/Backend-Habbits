namespace Habbits.Domain.Repositories.Habit;
public interface IHabitUpdateOnlyRepository
{
    Task<Entities.Habit> GetById(long id);
    void Update(Entities.Habit habit);
}
