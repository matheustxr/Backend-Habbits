using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Categories
{
    public interface ICategoryUpdateOnlyRepository
    {
        Task<HabitCategory?> GetById(User user, long id);
        void Update(HabitCategory category);
    }
}
