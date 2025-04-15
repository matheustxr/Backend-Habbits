using Habits.Domain.Repositories.Habits;
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
