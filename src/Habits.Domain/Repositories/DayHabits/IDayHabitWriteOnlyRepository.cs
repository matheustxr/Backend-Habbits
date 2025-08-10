using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.DayHabits
{
    public interface IDayHabitWriteOnlyRepository
    {
        Task AddAsync(DayHabit dayHabit);
    }
}
