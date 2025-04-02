using AutoMapper;
using FluentValidation.Results;
using Habbits.Communication.Requests.Users;
using Habbits.Communication.Responses.Users;
using Habbits.Domain.Repositories;
using Habbits.Domain.Repositories.Users;
using Habbits.Domain.Security.Cryptography;
using Habbits.Domain.Security.Tokens;
using Habbits.Exception;
using Habbits.Exception.ExceptionBase;

namespace Habbits.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncrypter _passwordEncripter;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUsertWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUnityOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _tokenGenerator;

    public RegisterUserUseCase(
        IMapper mapper,
        IPasswordEncrypter passwordEncripter,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUsertWriteOnlyRepository userWriteOnlyRepository,
        IAccessTokenGenerator tokenGenerator,
        IUnityOfWork unitOfWork)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);

        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _userWriteOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
