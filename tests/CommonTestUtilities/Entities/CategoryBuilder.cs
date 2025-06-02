using Bogus;
using Habits.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class CategoryBuilder
    {
        public static List<HabitCategory> Collection(User user, uint count = 2)
        {
            var list = new List<HabitCategory>();

            if (count == 0)
                count = 1;

             var categoryId = 0;

            for (int i = 0; i < count; i++)
            {
                var category = Build(user);
                category.Id = categoryId++;
                category.HexColor = "#00000";
            }

            return list;
        }

        public static HabitCategory Build(User user)
        {
            return new Faker<HabitCategory>()
                .RuleFor(c => c.Id, faker => faker.UniqueIndex)
                .RuleFor(c => c.Category, faker => faker.Commerce.Department())
                .RuleFor(c => c.UserId, _ => user.Id)
                .RuleFor(c => c.User, _ => user)
                .RuleFor(c => c.HexColor, faker => faker.Internet.Color());
        }
    }
}
