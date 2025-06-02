using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.Categories;
using FluentAssertions;
using Habits.Application.UseCases.Categories.GetAll;
using Habits.Application.UseCases.Categories.GetById;
using Habits.Domain.Entities;
using Habits.Exception.ExceptionBase;
using Habits.Exception;

namespace UseCases.Test.Categories.GetById
{
    public class GetCategoryByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var category = CategoryBuilder.Build(user);

            var useCase = CreateUseCase(user, category);

            var result = await useCase.Execute(category.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(category.Id);
            result.Category.Should().NotBeNullOrEmpty();
            result.HexColor.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Habit_Not_Found()
        {
            var user = UserBuilder.Build();
            var existingCategory = CategoryBuilder.Build(user);

            var useCase = CreateUseCase(user, existingCategory);

            var noneExistentId = existingCategory.Id + 9999;
            var act = async () => await useCase.Execute(id: noneExistentId);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.CATEGORY_NOT_FOUND));
        }

        private GetCategoryByIdUseCase CreateUseCase(User user, HabitCategory category)
        {
            var repository = new CategoriesReadOnlyRepositoryBuilder().GetById(user, category).Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            var mapper = MapperBuilder.Build();

            return new GetCategoryByIdUseCase(loggedUser, repository, mapper);
        }
    }
}
