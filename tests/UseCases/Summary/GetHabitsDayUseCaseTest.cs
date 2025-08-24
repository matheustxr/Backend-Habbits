using AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.DayHabit;
using FluentAssertions;
using Habits.Application.UseCases.Summary.GetHabitsDay;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Services.LoggedUser;

namespace UseCases.Test.Summary
{
    public class GetHabitsDayUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var date = new DateOnly(2025, 1, 1);
            var user = UserBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();

            var habitsResultTuple = new List<(long habitId, string title, string? categoryName, bool isCompleted)>
            {
                (1, "Ler Livro", "Leitura", true),
                (2, "Correr", "Exercício", false)
            };

            var repository = new DayHabitReadOnlyRepositoryBuilder().GetHabitsForDate(habitsResultTuple).Build();

            var useCase = new GetHabitsDayUseCase(repository, mapper, loggedUser);

            var response = await useCase.Execute(date);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<ResponseSummaryHabitJson>>();
            response.Should().HaveCount(habitsResultTuple.Count);
        }
    }
}
