using Habits.Communication.Requests.Users;

namespace Habits.Application.UseCases.Users.Update;
public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
