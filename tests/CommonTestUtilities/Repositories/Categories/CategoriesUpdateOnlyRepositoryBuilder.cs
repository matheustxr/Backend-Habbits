using Habits.Domain.Entities;
using Habits.Domain.Repositories.Categories;
using Moq;

namespace CommonTestUtilities.Repositories.Categories
{
    public class CategoriesUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<ICategoriesUpdateOnlyRepository> _repository;

        public CategoriesUpdateOnlyRepositoryBuilder()
        {
            _repository = new Mock<ICategoriesUpdateOnlyRepository>();

            _repository.Setup(repo => repo.Update(It.IsAny<HabitCategory>()));
        }

        public ICategoriesUpdateOnlyRepository Build() => _repository.Object;
    }
}
