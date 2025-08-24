using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.DayHabit;
using FluentAssertions;
using Habits.Application.UseCases.Summary.GetMounthly;
using Habits.Communication.Responses.Summary;
using Habits.Domain.Entities;

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

        private GetDateRangeSummaryUseCase CreateUseCase(User user, Dictionary<DateOnly, (int possible, int completed)> summaryData)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var repository = new DayHabitReadOnlyRepositoryBuilder().GetMonthlySummary(summaryData).Build();

            return new GetDateRangeSummaryUseCase(repository, mapper, loggedUser);
        }
    }
}
