using Habits.Communication.Requests.Users;
using Habits.Communication.Responses.Users;

namespace Habits.Application.UseCases.Login;
public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
