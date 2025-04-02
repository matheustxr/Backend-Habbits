

using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests.User;
using FluentAssertions;
using Habbits.Application.UseCases.Users.ChangePassword;
using Habbits.Domain.Entities;
using Habbits.Exception;
using Habbits.Exception.ExceptionBase;

namespace UseCases.Test.Users.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Password);

            var act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            var user = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            request.NewPassword = string.Empty;

            var useCase = CreateUseCase(user, request.Password);

            var act = async () => { await useCase.Execute(request); };

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(e => e.GetErrors().Count == 1 &&
                    e.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            var user = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            var act = async () => { await useCase.Execute(request); };

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(e => e.GetErrors().Count == 1 &&
                    e.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        private static ChangePasswordUseCase CreateUseCase(User user, string? password = null)
        {
            var unitOfWork = UnityOfWorkBuilder.Build();

            var userUpdateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);

            var loggedUser = LoggedUserBuilder.Build(user);

            var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();

            return new ChangePasswordUseCase(loggedUser, userUpdateRepository, unitOfWork, passwordEncrypter);
        }
    }
}
