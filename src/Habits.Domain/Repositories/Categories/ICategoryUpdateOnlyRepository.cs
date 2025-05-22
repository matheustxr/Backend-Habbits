using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Categories
{
    public interface ICategoryUpdateOnlyRepository
    {
        void Update(HabitCategory category);
    }
}
