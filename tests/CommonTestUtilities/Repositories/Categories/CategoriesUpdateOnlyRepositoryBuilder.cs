using Habits.Domain.Entities;
using Habits.Domain.Repositories.Categories;
using Moq;

namespace CommonTestUtilities.Repositories.Categories
{
    public static class CategoriesUpdateOnlyRepositoryBuilder
    {
        public static ICategoriesUpdateOnlyRepository Build()
        {
            var mock = new Mock<ICategoriesUpdateOnlyRepository>();

            mock.Setup(repo => repo.Update(It.IsAny<HabitCategory>()));

            return mock.Object;
        }
    }
}
