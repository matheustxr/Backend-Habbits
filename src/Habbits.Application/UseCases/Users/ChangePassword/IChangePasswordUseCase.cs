using Habbits.Communication.Requests.User;

namespace Habbits.Application.UseCases.Users.ChangePassword;
public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
