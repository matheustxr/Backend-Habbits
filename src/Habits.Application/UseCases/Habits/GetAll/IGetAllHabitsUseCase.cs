using Habits.Communication.Responses.Habits;

namespace Habits.Application.UseCases.Habits.GetAll
{
    public interface IGetAllHabitsUseCase
    {
        Task<ResponseHabitsJson> Execute();
    }
}
