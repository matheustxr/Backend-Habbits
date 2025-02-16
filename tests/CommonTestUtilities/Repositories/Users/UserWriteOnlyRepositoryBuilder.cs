using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Habbits.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories.Users
{
    public class UserWriteOnlyRepositoryBuilder
    {
        public static IUsertWriteOnlyRepository Build()
        {
            var mock = new Mock<IUsertWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
