
using Habbits.Domain.Repositories;
using Habbits.Domain.Repositories.User;
using Habbits.Domain.Services.LoggedUser;

namespace Habbits.Application.UseCases.Users.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IUserWriteOnlyRepository _repository;

    public DeleteUserAccountUseCase(
        ILoggedUser loggedUser,
        IUnityOfWork unityOfWork,
        IUserWriteOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _unityOfWork = unityOfWork;
        _repository = repository;

    }

    public async Task Execute()
    {
        var user = await _loggedUser.Get();

        await _repository.Delete(user);

        await _unityOfWork.Commit();
    }
} 
