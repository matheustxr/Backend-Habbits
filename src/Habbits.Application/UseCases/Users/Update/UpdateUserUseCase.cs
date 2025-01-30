using FluentValidation.Results;
using Habbits.Communication.Requests.User;
using Habbits.Domain.Repositories;
using Habbits.Domain.Repositories.User;
using Habbits.Domain.Services.LoggedUser;
using Habbits.Exception;
using Habbits.Exception.ExceptionBase;

namespace Habbits.Application.UseCases.Users.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnityOfWork _unitOfWork;

    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnityOfWork unityOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unityOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Email);

        var user = await _repository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _repository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (currentEmail.Equals(request.Email) == false)
        {
            var userExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
