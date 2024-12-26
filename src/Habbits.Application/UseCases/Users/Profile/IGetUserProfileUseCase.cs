using Habbits.Communication.Responses.Users;

namespace Habbits.Application.UseCases.Users.Profile;
public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
