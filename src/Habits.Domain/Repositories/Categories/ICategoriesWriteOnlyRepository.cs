using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Categories
{
    public interface ICategoriesWriteOnlyRepository
    {
        Task Add(HabitCategory category);
        Task Delete(User user, long id);
    }
}
