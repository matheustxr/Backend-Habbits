using Habbits.Communication.Requests.Habits;
using Habbits.Communication.Responses.Habbits;

namespace Habbits.Application.UseCases.Habit.Create;
public interface ICreateHabitUseCase
{
    Task<ResponseCreateHabitJson> Execute(RequestCreateHabitJson request);
};

