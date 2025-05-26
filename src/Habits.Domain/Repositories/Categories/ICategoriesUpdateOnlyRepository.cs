using Habits.Domain.Entities;

namespace Habits.Domain.Repositories.Categories
{
    public interface ICategoriesUpdateOnlyRepository
    {
        void Update(HabitCategory category);
    }
}
