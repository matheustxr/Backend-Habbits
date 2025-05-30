﻿using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Users;
using Habits.Domain.Services.LoggedUser;

namespace Habits.Application.UseCases.Users.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IUsertWriteOnlyRepository _repository;

    public DeleteUserAccountUseCase(
        ILoggedUser loggedUser,
        IUnityOfWork unityOfWork,
        IUsertWriteOnlyRepository repository)
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
