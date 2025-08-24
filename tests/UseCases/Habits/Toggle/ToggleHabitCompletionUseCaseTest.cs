using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.DayHabit;
using CommonTestUtilities.Repositories.Habits;
using FluentAssertions;
using Habits.Application.UseCases.Habits.ToggleCompletion;
using Habits.Domain.Entities;
using Habits.Domain.Repositories;
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

            var (useCase, dayHabitWriteRepo, unityOfWork) = CreateUseCase(user, habit);

            await useCase.Execute(habit.Id, date);

            dayHabitWriteRepo.GetMock()
                .Verify(repo => repo.ToggleCompletionStatusAsync(habit.Id, date), Times.Once);

            Mock.Get(unityOfWork).Verify(uow => uow.Commit(), Times.Once);
        }

        private static (ToggleHabitCompletionUseCase useCase, DayHabitWriteRepositoryBuilder writeRepo, IUnityOfWork unityOfWork)
            CreateUseCase(User user, Habit habit)
        {
            var habitReadRepo = new HabitsReadOnlyRepositoryBuilder().GetById(user, habit).Build();
            var writeRepo = new DayHabitWriteRepositoryBuilder();
            var unityOfWork = UnityOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            var useCase = new ToggleHabitCompletionUseCase(
                loggedUser,
                habitReadRepo,
                writeRepo.Build(),
                unityOfWork
            );

            return (useCase, writeRepo, unityOfWork);
        }

        private static DateOnly GetNextDateForWeekday(DayOfWeek targetDay)
        {
            var today = DateTime.Today;
            int daysToAdd = ((int)targetDay - (int)today.DayOfWeek + 7) % 7;
            if (daysToAdd == 0) daysToAdd = 7;
            return DateOnly.FromDateTime(today.AddDays(daysToAdd));
        }
    }
}
