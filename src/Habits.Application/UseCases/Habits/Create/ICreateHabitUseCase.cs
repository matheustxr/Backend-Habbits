using Habits.Communication.Requests.Habits;
using Habits.Communication.Responses.Habits;

namespace Habits.Application.UseCases.Habits.Create;
public interface ICreateHabitUseCase
{
    Task<ResponseCreateHabitJson> Execute(RequestCreateHabitJson request);
};

