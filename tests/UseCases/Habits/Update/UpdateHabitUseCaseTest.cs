using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Categories;
using CommonTestUtilities.Repositories.Habits;
using CommonTestUtilities.Requests.Habits;
using FluentAssertions;
using Habits.Application.UseCases.Habits.Update;
using Habits.Domain.Entities;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace UseCases.Test.Habits.Update;
public class UpdateHabitUseCaseTest
{


    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();

        var habit = HabitBuilder.Build(loggedUser);

        var request = RequestHabitJsonBuilder.Build();
        request.UserId = habit.UserId;

        var useCase = CreateUseCase(loggedUser, habit);

        var act = async () => await useCase.Execute(request, habit.Id);

        await act.Should().NotThrowAsync();

        habit.UserId.Should().Be(request.UserId);
        habit.Title.Should().Be(request.Title);
        habit.Description.Should().Be(request.Description);
        habit.WeekDays.Should().BeEquivalentTo(request.WeekDays);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var habit = HabitBuilder.Build(loggedUser);

        var request = RequestHabitJsonBuilder.Build();

        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser, habit);

        var act = async () => await useCase.Execute(request, habit.Id);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Which.GetErrors().Should().ContainSingle().And.Contain(ResourceErrorMessages.TITLE_EMPTY);
    }

    [Fact]
    public async Task Error_Habit_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var request = RequestHabitJsonBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request, id: 1000);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.HABIT_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var habit = HabitBuilder.Build(loggedUser);

        var request = RequestHabitJsonBuilder.Build();
        request.UserId = loggedUser.Id;
        request.CategoryId = 999;

        var habitReadOnlyRepositoryBuilder = new HabitsReadOnlyRepositoryBuilder();
        habitReadOnlyRepositoryBuilder.GetById(loggedUser, habit);

        var habitUpdateRepositoryBuilder = new HabitsUpdateOnlyRepositoryBuilder();
        habitUpdateRepositoryBuilder.GetById(loggedUser, habit);

        var categoryRepositoryBuilder = new CategoriesReadOnlyRepositoryBuilder();

        var mapper = MapperBuilder.Build();
        var unitOfWork = UnityOfWorkBuilder.Build();
        var loggedUserService = LoggedUserBuilder.Build(loggedUser);

        var useCase = new UpdateHabitUseCase(
            mapper,
            habitReadOnlyRepositoryBuilder.Build(),
            habitUpdateRepositoryBuilder.Build(),
            categoryRepositoryBuilder.Build(),
            loggedUserService,
            unitOfWork
        );

        var act = async () => await useCase.Execute(request, habit.Id);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Which.GetErrors().Should()
            .ContainSingle()
            .And.Contain(ResourceErrorMessages.CATEGORY_NOT_FOUND);
    }

    private UpdateHabitUseCase CreateUseCase(User user, Habit? habit = null)
    {
        var readOnlyRepositoryBuilder = new HabitsReadOnlyRepositoryBuilder();
        var categoryReadOnlyRepository = new CategoriesReadOnlyRepositoryBuilder().Build();

        if (habit is not null)
            readOnlyRepositoryBuilder.GetById(user, habit);

        var readOnlyRepository = readOnlyRepositoryBuilder.Build();

        var updateRepositoryBuilder = new HabitsUpdateOnlyRepositoryBuilder();

        if (habit is not null)
            updateRepositoryBuilder.GetById(user, habit);

        var habitUpdateRepository = updateRepositoryBuilder.Build();

        var mapper = MapperBuilder.Build();
        var unitOfWork = UnityOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateHabitUseCase(mapper, readOnlyRepository, habitUpdateRepository, categoryReadOnlyRepository, loggedUser, unitOfWork);
    }
}