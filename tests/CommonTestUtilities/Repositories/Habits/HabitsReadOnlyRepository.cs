using Habbits.Domain.Repositories.Habit;
using Moq;

namespace CommonTestUtilities.Repositories.Habits
{
    public class HabitsReadOnlyRepository
    {
        public static IHabitReadOnlyRepository Build()
        {
            var mock = new Mock<IHabitReadOnlyRepository>();

            return mock.Object;
        }
    }
}
