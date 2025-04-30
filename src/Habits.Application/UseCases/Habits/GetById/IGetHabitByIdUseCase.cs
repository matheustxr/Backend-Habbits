using Habits.Communication.Responses.Habits;

namespace Habits.Application.UseCases.Habits.GetById
{
    public interface IGetHabitByIdUseCase
    {
        Task<ResponseHabitJson> Execute(long id);
    }
}
