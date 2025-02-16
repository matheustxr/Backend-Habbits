using Habbits.Domain.Repositories.Habit;
using Moq;

namespace CommonTestUtilities.Repositories.Habits
{
    public class HabitsWriteOnlyRepositoryBuilder
    {
        public static IHabitWriteOnlyRepository Build()
        {
            var mock = new Mock<IHabitWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
