using Habbits.Domain.Repositories.Habits;
using Moq;

namespace CommonTestUtilities.Repositories.Habits
{
    public class HabitsReadOnlyRepositoryBuilder
    {
        public static IHabitReadOnlyRepository Build()
        {
            var mock = new Mock<IHabitReadOnlyRepository>();

            return mock.Object;
        }
    }
}
