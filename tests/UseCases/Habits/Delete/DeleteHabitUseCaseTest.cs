using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using Habits.Domain.Entities;
using Habits.Exception.ExceptionBase;
using Habits.Exception;
using Habits.Application.UseCases.Habits.Delete;
using CommonTestUtilities.Repositories.Habits;

namespace UseCases.Test.Habits.Delete
{
    public class DeleteHabitUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();

            var habit = HabitBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, habit);

            var act = async () => await useCase.Execute(habit.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Habit_Not_Found()
        {
            var loggedUser = UserBuilder.Build();
            var existingHabit = HabitBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, existingHabit);
            var noneExistentId = existingHabit.Id + 9999;

            var act = async () => await useCase.Execute(id: noneExistentId);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.HABIT_NOT_FOUND));
        }

        private DeleteHabitUseCase CreateUseCase(User user, Habit habit)
        {
            var repositoryReadOnly = new HabitsReadOnlyRepositoryBuilder().GetById(user, habit).Build();    

            var repositoryWriteOnly = HabitsWriteOnlyRepositoryBuilder.Build();

            var unitOfWork = UnityOfWorkBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new DeleteHabitUseCase(repositoryReadOnly, repositoryWriteOnly, unitOfWork, loggedUser);
        }
    }
}
