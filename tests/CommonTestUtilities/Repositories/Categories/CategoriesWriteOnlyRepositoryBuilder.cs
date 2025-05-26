using Habits.Domain.Repositories.Categories;
using Moq;

namespace CommonTestUtilities.Repositories.Categories
{
    public class CategoriesWriteOnlyRepositoryBuilder
    {
        private readonly Mock<ICategoriesWriteOnlyRepository> _repository;

        public CategoriesWriteOnlyRepositoryBuilder()
        {
            _repository = new Mock<ICategoriesWriteOnlyRepository>();
        }

        public ICategoriesWriteOnlyRepository Build() => _repository.Object;
    }
}
