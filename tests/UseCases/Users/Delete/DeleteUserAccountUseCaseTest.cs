using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using FluentAssertions;
using Habbits.Application.UseCases.Users.Delete;
using Habbits.Domain.Entities;

namespace UseCases.Test.Users.Delete
{
    public class DeleteUserAccountUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute();

            await act.Should().NotThrowAsync();
        }

        private DeleteUserAccountUseCase CreateUseCase(User user)
        {
            var repository = UsertWriteOnlyRepositoryBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            var unitOfWork = UnityOfWorkBuilder.Build();

            return new DeleteUserAccountUseCase(loggedUser, unitOfWork, repository);
        }
    }
}
