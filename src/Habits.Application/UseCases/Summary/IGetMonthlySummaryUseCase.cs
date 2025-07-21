using Habits.Communication.Responses.Summary;

namespace Habits.Application.UseCases.Summary
{
    public interface IGetMonthlySummaryUseCase
    {
        Task<List<ResponseSummaryJson>> ExecuteAsync(Guid userId, int year, int month);
    }
}
