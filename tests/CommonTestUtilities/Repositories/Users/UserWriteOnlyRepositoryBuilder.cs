using Moq;

namespace CommonTestUtilities.Repositories.Users
{
    public class IUsertWriteOnlyRepository
    {
        public static IUsertWriteOnlyRepository Build()
        {
            var mock = new Mock<IUsertWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
