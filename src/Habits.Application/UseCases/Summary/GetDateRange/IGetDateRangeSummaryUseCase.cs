using Habits.Communication.Responses.Summary;

namespace Habits.Application.UseCases.Summary.GetMounthly
{
    public interface IGetDateRangeSummaryUseCase
    {
        Task<List<ResponseSummaryJson>> Execute(DateOnly startDate, DateOnly endDate);
    }
}
