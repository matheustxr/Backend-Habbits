using Habbits.Communication.Requests.User;

namespace Habbits.Application.UseCases.Users.Update;
public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
