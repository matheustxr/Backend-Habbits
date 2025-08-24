using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.DayHabit;
using CommonTestUtilities.Repositories.Habits;
using FluentAssertions;
using Habits.Application.UseCases.Habits.ToggleCompletion;
using Habits.Domain.Entities;
using Habits.Domain.Repositories;
using Habits.Exception.ExceptionBase;
using Habits.Exception;
using Moq;

namespace UseCases.Test.Habits.Toggle
{
    public class ToggleHabitCompletionUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var habit = HabitBuilder.Build(user);
            var date = GetNextDateForWeekday(DayOfWeek.Monday);

            habit.IsActive = true;

            var (useCase, writeRepoBuilder, unitOfWork) = CreateUseCase(user, habit);

            await useCase.Execute(habit.Id, date);

            writeRepoBuilder.GetMock()
                .Verify(repo => repo.ToggleCompletionStatusAsync(habit.Id, date), Times.Once);

            Mock.Get(unitOfWork)
                .Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Habit_Is_Inactive()
        {
            var user = UserBuilder.Build();
            var habit = HabitBuilder.Build(user);
            habit.IsActive = false;

            var (useCase, _, _) = CreateUseCase(user, habit);

            var date = GetNextDateForWeekday(DayOfWeek.Monday);

            Func<Task> act = async () => await useCase.Execute(habit.Id, date);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage(ResourceErrorMessages.HABIT_NOT_ACTIVE);
        }

        [Fact]
        public async Task Should_Throw_ValidationException_When_Habit_Is_Not_Scheduled_For_Day()
        {
            var user = UserBuilder.Build();
            var habit = HabitBuilder.Build(user);
            habit.WeekDays.Clear();

            var (useCase, _, _) = CreateUseCase(user, habit);
            var date = GetNextDateForWeekday(DayOfWeek.Monday);

            Func<Task> act = async () => await useCase.Execute(habit.Id, date);

            var exception = await act.Should()
                .ThrowAsync<ErrorOnValidationException>();

            exception.Which.GetErrors()
                .Should().Contain(ResourceErrorMessages.HABIT_NOT_TODAY);
        }

        private static (
            ToggleHabitCompletionUseCase useCase, 
            DayHabitWriteRepositoryBuilder writeRepoBuilder, 
            IUnityOfWork unitOfWork)
        CreateUseCase(User user, Habit habit)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var habitReadRepo = new HabitsReadOnlyRepositoryBuilder().GetById(user, habit).Build();
            var writeRepoBuilder = new DayHabitWriteRepositoryBuilder();
            var unitOfWork = UnityOfWorkBuilder.Build();

            var useCase = new ToggleHabitCompletionUseCase(
                loggedUser,
                habitReadRepo,
                writeRepoBuilder.Build(),
                unitOfWork
            );

            return (useCase, writeRepoBuilder, unitOfWork);
        }

        private static DateOnly GetNextDateForWeekday(DayOfWeek targetDay)
        {
            var today = DateTime.Today;
            int daysToAdd = ((int)targetDay - (int)today.DayOfWeek + 7) % 7;
            if (daysToAdd == 0) daysToAdd = 7;

            var nextDate = today.AddDays(daysToAdd);
            return DateOnly.FromDateTime(nextDate);
        }
    }
}
