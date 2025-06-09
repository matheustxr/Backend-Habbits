using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Categories;
using FluentAssertions;
using Habits.Application.UseCases.Categories.Delete;
using Habits.Domain.Entities;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace UseCases.Test.Categories.Delete
{
    public class DeleteCategoryUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var category = CategoryBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, category);

            var act = async () => await useCase.Execute(category.Id);

            await act.Should().NotThrowAsync();

            category.UserId.Should().Be(loggedUser.Id);
        }

        [Fact]
        public async Task Error_Category_Not_Found()
        {
            var loggedUser = UserBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(id: 1000);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.CATEGORY_NOT_FOUND));
        }

        private DeleteCategoryUseCase CreateUseCase(User user, HabitCategory? category = null)
        {
            var readRepositoryBuilder = new CategoriesReadOnlyRepositoryBuilder();

            if (category is not null)
                readRepositoryBuilder.GetById(user, category);

            var readOnlyRepository = readRepositoryBuilder.Build();

            var writeOnlyRepository = new CategoriesWriteOnlyRepositoryBuilder().Build();

            var unitOfWork = UnityOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new DeleteCategoryUseCase(readOnlyRepository, writeOnlyRepository, unitOfWork, loggedUser);
        }
    }
}
