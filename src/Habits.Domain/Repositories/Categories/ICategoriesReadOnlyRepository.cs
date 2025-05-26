using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Categories
{
    public interface ICategoriesReadOnlyRepository
    {
        Task<List<HabitCategory>> GetAll(User user);
        Task<HabitCategory?> GetById(User user, long id);
        Task<bool> AlreadyCategoryExist(string category, Guid userId, long? excludeId = null);
        Task<HabitCategory?> GetByCategory(string category, Guid userId);
    }
}
