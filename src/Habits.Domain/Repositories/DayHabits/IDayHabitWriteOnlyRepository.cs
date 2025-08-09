using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.DayHabits
{
    public interface IDayHabitWriteOnlyRepository
    {
        Task ToggleCompletionStatusAsync(long habitId, DateOnly date);
    }
}
