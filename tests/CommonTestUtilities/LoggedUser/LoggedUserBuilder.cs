﻿using Habbits.Domain.Entities;
using Habbits.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            var mock = new Mock<ILoggedUser>();

            mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
