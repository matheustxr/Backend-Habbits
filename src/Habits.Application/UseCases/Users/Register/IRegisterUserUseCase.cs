using Habits.Communication.Requests.Users;
using Habits.Communication.Responses.Users;

namespace Habits.Application.UseCases.Users.Register;
public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}

