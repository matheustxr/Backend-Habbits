using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Categories;
using CommonTestUtilities.Repositories.Habits;
using CommonTestUtilities.Requests.Habits;
using FluentAssertions;
using Habits.Application.UseCases.Habits.Create;
using Habits.Application.UseCases.Habits.Update;
using Habits.Domain.Entities;
using Habits.Domain.Repositories.Habits;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace UseCases.Test.Habits.Create;

public class CreateHabitUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();

        var request = RequestHabitJsonBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var request = RequestHabitJsonBuilder.Build();

        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Which.GetErrors().Should().ContainSingle().And.Contain(ResourceErrorMessages.TITLE_EMPTY);
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var habit = HabitBuilder.Build(loggedUser);

        var request = RequestHabitJsonBuilder.Build();
        request.UserId = loggedUser.Id;
        request.CategoryId = 999;

        var habitsReadOnlyRepository = new HabitsReadOnlyRepositoryBuilder();
        habitsReadOnlyRepository.GetById(loggedUser, habit);

        var habitsWriteOnlyRepository = new HabitsWriteOnlyRepositoryBuilder();

        var categoryReadOnlyRepository = new CategoriesReadOnlyRepositoryBuilder();

        var mapper = MapperBuilder.Build();
        var unitOfWork = UnityOfWorkBuilder.Build();
        var loggedUserService = LoggedUserBuilder.Build(loggedUser);

        var useCase = new CreateHabitUseCase(
            mapper,
            habitsReadOnlyRepository.Build(),
            habitsWriteOnlyRepository.Build(),
            categoryReadOnlyRepository.Build(),
            loggedUserService,
            unitOfWork
        );

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Which.GetErrors().Should()
            .ContainSingle()
            .And.Contain(ResourceErrorMessages.CATEGORY_NOT_FOUND);
    }

    private CreateHabitUseCase CreateUseCase(User user)
    {
        var habitsReadOnlyRepository = new HabitsReadOnlyRepositoryBuilder().Build();
        var habitsWriteOnlyRepository = new HabitsWriteOnlyRepositoryBuilder().Build();
        var categoryReadOnlyRepository = new CategoriesReadOnlyRepositoryBuilder().Build();

        var mapper = MapperBuilder.Build();

        var unitOfWork = UnityOfWorkBuilder.Build();

        var loggedUser = LoggedUserBuilder.Build(user);

        return new CreateHabitUseCase(mapper, habitsReadOnlyRepository, habitsWriteOnlyRepository, categoryReadOnlyRepository, loggedUser, unitOfWork);
    }
}
