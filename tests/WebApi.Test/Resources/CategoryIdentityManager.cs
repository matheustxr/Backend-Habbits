using Habits.Domain.Entities;

namespace WebApi.Test.Resources
{
    public class CategoryIdentityManager
    {
        private readonly HabitCategory _category;

        public CategoryIdentityManager(HabitCategory category)
        {
            _category = category;
        }

        public long GetId() => _category.Id;
    }
}
