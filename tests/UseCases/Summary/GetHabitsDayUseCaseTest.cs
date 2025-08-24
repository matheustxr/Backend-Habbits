using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.DayHabit;
using FluentAssertions;
using Habits.Application.UseCases.Summary.GetHabitsDay;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Entities;
using Habits.Domain.Services.LoggedUser;
using Moq;

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

        [Fact]
        public async Task Should_Return_EmptyList_When_NoHabitsForDate()
        {
            var user = UserBuilder.Build();
            var date = new DateOnly(2025, 1, 2);

            var useCase = CreateUseCase(user, new List<(long, string, string?, bool)>());

            var response = await useCase.Execute(date);

            response.Should().NotBeNull();
            response.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Call_GetLoggedUser_Once()
        {
            var user = UserBuilder.Build();
            var date = new DateOnly(2025, 1, 3);

            var loggedUserMock = new Mock<ILoggedUser>();
            loggedUserMock.Setup(lu => lu.Get()).ReturnsAsync(user);

            var mapper = MapperBuilder.Build();
            var repository = new DayHabitReadOnlyRepositoryBuilder().GetHabitsForDate(new List<(long, string, string?, bool)>()).Build();

            var useCase = new GetHabitsDayUseCase(repository, mapper, loggedUserMock.Object);

            await useCase.Execute(date);

            loggedUserMock.Verify(lu => lu.Get(), Times.Once);
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
