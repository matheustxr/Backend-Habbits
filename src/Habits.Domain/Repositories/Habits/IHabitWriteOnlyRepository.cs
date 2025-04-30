using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Habits;
public interface IHabitWriteOnlyRepository
{
    Task Add(Habit habit);
    Task Delete(User user, long id);
}