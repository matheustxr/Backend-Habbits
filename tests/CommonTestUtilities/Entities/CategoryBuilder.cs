using Bogus;
using Habits.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class CategoryBuilder
    {
        public static HabitCategory Build(User user)
        {
            return new Faker<HabitCategory>()
                .RuleFor(c => c.Id, faker => faker.UniqueIndex)
                .RuleFor(c => c.Category, faker => faker.Commerce.Department())
                .RuleFor(c => c.UserId, _ => user.Id)
                .RuleFor(c => c.User, _ => user)
                .RuleFor(c => c.HexColor, f => f.Internet.Color());
        }
    }
}
