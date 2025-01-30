using Habbits.Communication.Requests.User;
using Habbits.Communication.Responses.Users;

namespace Habbits.Application.UseCases.Login;
public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
