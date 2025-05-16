using Habits.Communication.Requests.Habits;

namespace Habits.Application.UseCases.Habits.Update
{
    public interface IUpdateHabitUseCase
    {
        Task Execute(RequestHabitJson request, long id);
    }
}
