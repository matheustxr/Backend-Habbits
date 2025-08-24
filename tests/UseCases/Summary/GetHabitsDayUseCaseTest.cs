using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.DayHabit;
using FluentAssertions;
using Habits.Application.UseCases.Summary.GetHabitsDay;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Entities;

namespace UseCases.Test.Summary
{
    public class GetHabitsDayUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var date = new DateOnly(2025, 1, 1);

            var habits = new List<(long habitId, string title, string? categoryName, bool isCompleted)>
            {
                (1, "Ler Livro", "Leitura", true),
                (2, "Correr", "Exercício", false)
            };

            var useCase = CreateUseCase(user, habits);

            var response = await useCase.Execute(date);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<ResponseSummaryHabitJson>>();
            response.Should().HaveCount(habits.Count);
        }

        private GetHabitsDayUseCase CreateUseCase(User user, List<(long habitId, string title, string? categoryName, bool isCompleted)> habits)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var repository = new DayHabitReadOnlyRepositoryBuilder().GetHabitsForDate(habits).Build();

            return new GetHabitsDayUseCase(repository, mapper, loggedUser);
        }
    }
}
