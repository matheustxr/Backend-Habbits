using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Categories
{
    public interface ICategoryWriteOnlyRepository
    {
        Task Add(HabitCategory category);
        Task Delete(User user, long id);
    }
}
