using Habits.Communication.Responses.Summary;

namespace Habits.Application.UseCases.Summary.GetMounthly
{
    public interface IGetMonthlySummaryUseCase
    {
        Task<List<ResponseSummaryJson>> Execute(DateOnly startDate, DateOnly endDate);
    }
}
