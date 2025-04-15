using Habits.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UnityOfWorkBuilder
    {
        public static IUnityOfWork Build()
        {
            var mock = new Mock<IUnityOfWork>();

            return mock.Object;
        }
    }
}
