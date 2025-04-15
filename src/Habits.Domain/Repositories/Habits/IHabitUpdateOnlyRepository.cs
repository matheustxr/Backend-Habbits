namespace Habits.Domain.Repositories.Habits;
public interface IHabitUpdateOnlyRepository
{
    Task<Entities.Habit> GetById(long id);
    void Update(Entities.Habit habit);
}
