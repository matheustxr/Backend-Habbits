using Bogus;
using Habits.Communication.Requests.Categories;

namespace CommonTestUtilities.Requests.Categories
{
    public class RequestCategoryBuilder
    {
        public static RequestCategoryJson Build()
        {
            var faker = new Faker();

            var category = new RequestCategoryJson
            {
                Category = faker.Commerce.Categories(1).First(),
                HexColor = faker.Internet.Color()
            };

            return category;
        }
    }
}
