using Habits.Communication.Responses.Summary;

namespace Habits.Application.UseCases.Summary.GetHabitsDay
{
    public interface IGetHabitsDayUseCase
    {
        Task<List<ResponseSummaryHabitJson>> Execute(DateOnly date);
    }
}
