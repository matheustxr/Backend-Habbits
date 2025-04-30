using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Habits;
public interface IHabitUpdateOnlyRepository
{
    Task<Habit?> GetById(User user, long id);
    void Update(Habit habit);
}
