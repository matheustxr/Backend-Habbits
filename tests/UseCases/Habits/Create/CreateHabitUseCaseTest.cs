using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Habits;
using CommonTestUtilities.Requests.Habits;
using FluentAssertions;
using Habits.Application.UseCases.Habits.Create;
using Habits.Domain.Entities;
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

    private CreateHabitUseCase CreateUseCase(User user)
    {
        var readOnlyUpdateRepository = new HabitsReadOnlyRepositoryBuilder().Build();
        var writeOnlyRepository = new HabitsWriteOnlyRepositoryBuilder().Build();

        var mapper = MapperBuilder.Build();

        var unitOfWork = UnityOfWorkBuilder.Build();

        var loggedUser = LoggedUserBuilder.Build(user);

        return new CreateHabitUseCase(mapper, readOnlyUpdateRepository, writeOnlyRepository, loggedUser, unitOfWork);
    }
}
