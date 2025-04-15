using Bogus;
using Habits.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class HabitCategoryBuilder
    {
        public static HabitCategory Build()
        {
            return new Faker<HabitCategory>()
                .RuleFor(c => c.Id, _ => 1)
                .RuleFor(c => c.Category, faker => faker.Commerce.Department());
        }
    }
}
