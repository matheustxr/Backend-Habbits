using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.Habits;
using FluentAssertions;
using Habits.Application.UseCases.Habits.GetById;
using Habits.Domain.Entities;
using Habits.Exception;
using Habits.Exception.ExceptionBase;

namespace UseCases.Test.Habits.GetById
{
    public class GetHabitByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();

            var habit = HabitBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, habit);

            var result = await useCase.Execute(habit.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(habit.Id);
            result.Title.Should().Be(habit.Title);
            result.Description.Should().Be(habit.Description);
            result.WeekDays.Should().NotBeNullOrEmpty();
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

        private GetHabitByIdUseCase CreateUseCase(User user, Habit habit)
        {
            var repository = new HabitsReadOnlyRepositoryBuilder().GetById(user, habit).Build();

            var mapper = MapperBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetHabitByIdUseCase(repository, mapper, loggedUser);
        }
    }
}
