using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.Categories;
using FluentAssertions;
using Habits.Application.UseCases.Categories.GetAll;
using Habits.Domain.Entities;

namespace UseCases.Test.Categories.GetAll
{
    public class GetAllCategoriesUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();

            var categories = CategoryBuilder.Collection(loggedUser);

            var useCase = CreateUseCase(loggedUser, categories);

            var result = await useCase.Execute();

            result.Should().NotBeNull();
            result.Categories.Should().NotBeNullOrEmpty().And.AllSatisfy(category =>
            {
                category.Id.Should().BeGreaterThan(0);
                category.Category.Should().NotBeNullOrEmpty();
                category.HexColor.Should().NotBeNullOrEmpty();
            });
        }

        private GetAllCategoriesUseCase CreateUseCase(User user, List<HabitCategory> categories)
        {
            var repository = new CategoriesReadOnlyRepositoryBuilder().GetAll(user, categories).Build();

            var mapper = MapperBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetAllCategoriesUseCase(loggedUser, repository, mapper);
        }
    }
}
