using Habits.Communication.Responses.Summary;

namespace Habits.Application.UseCases.Summary
{
    public interface IGetMonthlySummaryUseCase
    {
        Task<List<ResponseSummaryJson>> Execute(Guid userId, DateOnly startDate, DateOnly endDate);
    }
}
