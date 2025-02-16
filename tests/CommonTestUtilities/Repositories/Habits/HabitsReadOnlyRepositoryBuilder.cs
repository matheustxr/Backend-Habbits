using Habbits.Domain.Repositories.Habit;
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
