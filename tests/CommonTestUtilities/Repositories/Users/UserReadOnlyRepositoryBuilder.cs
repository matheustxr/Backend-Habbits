﻿

using Habbits.Domain.Entities;
using Habbits.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories.User
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _repository;

        public UserReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }

        public void ExistActiveUserWithEmail(string email)
        {
            _repository.Setup(userReadOnly => userReadOnly.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
        }

        public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
        {
            _repository.Setup(userRepository => userRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);

            return this;
        }
    }
}
