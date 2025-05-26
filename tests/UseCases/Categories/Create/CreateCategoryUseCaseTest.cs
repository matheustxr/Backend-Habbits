using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Categories;
using CommonTestUtilities.Requests.Categories;
using FluentAssertions;
using Habits.Application.UseCases.Categories.Create;
using Habits.Domain.Entities;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace UseCases.Test.Categories.Create
{
    public class CreateCategoryUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestCategoryBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Category.Should().Be(request.Category);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestCategoryBuilder.Build();

            request.Category = string.Empty;

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Which.GetErrors().Should().ContainSingle().And.Contain(ResourceErrorMessages.TITLE_EMPTY);
        }

        private CreateCategoryUseCase CreateUseCase(User user)
        {
            var readOnlyRepository = new CategoriesReadOnlyRepositoryBuilder().Build();
            var writeOnlyRepository = new CategoriesWriteOnlyRepositoryBuilder().Build();

            var mapper = MapperBuilder.Build();

            var unitOfWork = UnityOfWorkBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new CreateCategoryUseCase(mapper, readOnlyRepository, writeOnlyRepository, loggedUser, unitOfWork);
        }
    }
}
