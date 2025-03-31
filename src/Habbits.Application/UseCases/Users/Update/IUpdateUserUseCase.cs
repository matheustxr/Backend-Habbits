using Habbits.Communication.Requests.Users;

namespace Habbits.Application.UseCases.Users.Update;
public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
