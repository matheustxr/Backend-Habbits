using Habbits.Communication.Requests.Users;
using Habbits.Communication.Responses.Users;

namespace Habbits.Application.UseCases.Login;
public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
