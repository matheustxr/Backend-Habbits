﻿using Habbits.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories.Users
{
    public class UsertWriteOnlyRepository
    {
        public static IUsertWriteOnlyRepository Build()
        {
            var mock = new Mock<IUsertWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
