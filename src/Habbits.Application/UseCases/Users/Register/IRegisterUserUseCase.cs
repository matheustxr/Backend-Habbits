using Habbits.Communication.Requests.Users;
using Habbits.Communication.Responses.Users;

namespace Habbits.Application.UseCases.Users.Register;
public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}

