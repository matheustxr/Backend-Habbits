using Habbits.Communication.Requests.Users;

namespace Habbits.Application.UseCases.Users.ChangePassword;
public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
