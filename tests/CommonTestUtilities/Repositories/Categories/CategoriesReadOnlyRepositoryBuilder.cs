using CommonTestUtilities.Repositories.Habits;
using Habits.Domain.Entities;
using Habits.Domain.Repositories.Categories;
using Moq;

namespace CommonTestUtilities.Repositories.Categories
{
    public class CategoriesReadOnlyRepositoryBuilder
    {
        private readonly Mock<ICategoryReadOnlyRepository> _repository;

        public CategoriesReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<ICategoryReadOnlyRepository>();
        }

        public CategoriesReadOnlyRepositoryBuilder GetAll(User user)
        {
            _repository.Setup(repository => repository.GetAll(user));

            return this;
        }

        public CategoriesReadOnlyRepositoryBuilder GetById(User user, HabitCategory category)
        {
            _repository
                .Setup(repository => repository.GetById(
                    It.Is<User>(u => u.Id == user.Id),
                    It.Is<long>(id => id == category.Id)))
                .ReturnsAsync(category);

            return this;
        }

        public ICategoryReadOnlyRepository Build() => _repository.Object;
    }
}
