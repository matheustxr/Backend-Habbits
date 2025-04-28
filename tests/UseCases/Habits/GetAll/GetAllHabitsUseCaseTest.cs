using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.Habits;
using FluentAssertions;
using Habits.Application.UseCases.Habits.GetAll;
using Habits.Domain.Entities;

namespace UseCases.Test.Habits.GetAll
{
    public class GetAllHabitsUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();

            var habits = HabitBuilder.Collection(loggedUser);

            var useCase = CreateUseCase(loggedUser, habits);

            var result = await useCase.Execute();

            result.Should().NotBeNull();
            result.Habits.Should().NotBeNullOrEmpty().And.AllSatisfy(habit =>
            {
                habit.Id.Should().BeGreaterThan(0);
                habit.Title.Should().NotBeNullOrEmpty();
            });
        }

        private GetAllHabitsUseCase CreateUseCase(User user, List<Habit> habits)
        {
            var repository = new HabitsReadOnlyRepositoryBuilder().GetAll(user, habits).Build();

            var mapper = MapperBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetAllHabitsUseCase(repository, mapper, loggedUser);
        }
    }
}
