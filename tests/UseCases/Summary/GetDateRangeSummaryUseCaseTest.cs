using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.DayHabit;
using FluentAssertions;
using Habits.Application.UseCases.Summary.GetMounthly;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Entities;
using Habits.Domain.Services.LoggedUser;
using Habits.Exception;
using Habits.Exception.ExceptionBase;
using Moq;

namespace UseCases.Test.Summary
{
    public class GetDateRangeSummaryUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var (startDate, endDate) = (new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));

            var summaryData = new Dictionary<DateOnly, (int possible, int completed)>
            {
                [new DateOnly(2025, 1, 1)] = (3, 2),
                [new DateOnly(2025, 1, 2)] = (2, 2)
            };

            var useCase = CreateUseCase(user, summaryData);

            var response = await useCase.Execute(startDate, endDate);

            response.Should().NotBeNull();
            response.Should().BeOfType<List<ResponseSummaryJson>>();
            response.Should().HaveCount(summaryData.Count);
        }

        [Fact]
        public async Task Should_Throw_ErrorOnValidationException_When_DateRange_Is_Invalid()
        {
            var user = UserBuilder.Build();
            var startDate = new DateOnly(2025, 2, 1);
            var endDate = new DateOnly(2025, 1, 1); // início > fim (inválido)

            var useCase = CreateUseCase(user, new Dictionary<DateOnly, (int possible, int completed)>());

            Func<Task> act = async () => await useCase.Execute(startDate, endDate);

            var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
            exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.END_DATE_CANT_BE_MINOR_OF_START_DATE);
        }

        [Fact]
        public async Task Should_Return_EmptyList_When_NoDataForDateRange()
        {
            var user = UserBuilder.Build();
            var startDate = new DateOnly(2025, 1, 1);
            var endDate = new DateOnly(2025, 1, 31);

            var useCase = CreateUseCase(user, new Dictionary<DateOnly, (int possible, int completed)>()); // vazio

            var result = await useCase.Execute(startDate, endDate);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Call_GetLoggedUser_Once()
        {
            var user = UserBuilder.Build();
            var startDate = new DateOnly(2025, 1, 1);
            var endDate = new DateOnly(2025, 1, 31);

            var loggedUserMock = new Mock<ILoggedUser>();
            loggedUserMock.Setup(lu => lu.Get()).ReturnsAsync(user);

            var repository = new DayHabitReadOnlyRepositoryBuilder().GetMonthlySummary(new Dictionary<DateOnly, (int possible, int completed)>()).Build();
            var mapper = MapperBuilder.Build();

            var useCase = new GetDateRangeSummaryUseCase(repository, mapper, loggedUserMock.Object);

            await useCase.Execute(startDate, endDate);

            loggedUserMock.Verify(lu => lu.Get(), Times.Once);
        }

        private GetDateRangeSummaryUseCase CreateUseCase(User user, Dictionary<DateOnly, (int possible, int completed)> summaryData)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var repository = new DayHabitReadOnlyRepositoryBuilder().GetMonthlySummary(summaryData).Build();

            return new GetDateRangeSummaryUseCase(repository, mapper, loggedUser);
        }
    }
}
