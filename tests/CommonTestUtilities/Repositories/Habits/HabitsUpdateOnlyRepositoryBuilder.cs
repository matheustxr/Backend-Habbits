using Habbits.Domain.Repositories.Habit;
using Moq;

namespace CommonTestUtilities.Repositories.Habits
{
    public class HabitsUpdateOnlyRepositoryBuilder
    {
        public static IHabitUpdateOnlyRepository Build()
        {
            var mock = new Mock<IHabitUpdateOnlyRepository>();

            return mock.Object;
        }
    }
}
