using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.Habits;
using CommonTestUtilities.Repositories;
using Habits.Application.UseCases.Habits.Update;
using Habits.Domain.Entities;
using Habits.Application.UseCases.Categories.Update;
using CommonTestUtilities.Repositories.Categories;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests.Habits;
using FluentAssertions;
using Habits.Exception.ExceptionBase;
using Habits.Exception;
using CommonTestUtilities.Requests.Categories;

namespace UseCases.Test.Categories.Update
{
    public class UpdateCategoryUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var category = CategoryBuilder.Build(loggedUser);
            var request = RequestCategoryBuilder.Build();

            var useCase = CreateUseCase(loggedUser, category);

            var act = async () => await useCase.Execute(request, category.Id);

            await act.Should().NotThrowAsync();

            category.UserId.Should().Be(loggedUser.Id);
            category.Category.Should().Be(request.Category);
            category.HexColor.Should().Be(request.HexColor);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var loggedUser = UserBuilder.Build();

            var category = CategoryBuilder.Build(loggedUser);

            var request = RequestCategoryBuilder.Build();

            request.Category = string.Empty;

            var useCase = CreateUseCase(loggedUser, category);

            var act = async () => await useCase.Execute(request, category.Id);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Which.GetErrors().Should().ContainSingle().And.Contain(ResourceErrorMessages.TITLE_EMPTY);
        }

        [Fact]
        public async Task Error_Category_Not_Found()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestCategoryBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(request, id: 1000);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.CATEGORY_NOT_FOUND));
        }

        private UpdateCategoryUseCase CreateUseCase(User user, HabitCategory? category = null)
        {
            var readOnlyRepositoryBuilder = new CategoriesReadOnlyRepositoryBuilder();

            if (category is not null)
                readOnlyRepositoryBuilder.GetById(user, category);

            var readOnlyRepository = readOnlyRepositoryBuilder.Build();

            var categoryUpdateRepository = CategoriesUpdateOnlyRepositoryBuilder.Build();

            var mapper = MapperBuilder.Build();
            var unitOfWork = UnityOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new UpdateCategoryUseCase(mapper, readOnlyRepository, categoryUpdateRepository, loggedUser, unitOfWork);
        }
    }
}
