using Habbits.Domain.Entities;
using Habbits.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories.Users
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        public static IUserUpdateOnlyRepository Build(User user)
        {
            var mock = new Mock<IUserUpdateOnlyRepository>();

            mock.Setup(repositoy => repositoy.GetById(user.Id)).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
