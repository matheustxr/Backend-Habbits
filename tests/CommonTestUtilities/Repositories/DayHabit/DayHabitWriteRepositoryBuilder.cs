using Habits.Domain.Repositories.DayHabits;
using Moq;

namespace CommonTestUtilities.Repositories.DayHabit
{
    public class DayHabitWriteRepositoryBuilder
    {
        private readonly Mock<IDayHabitWriteOnlyRepository> _repository;

        public DayHabitWriteRepositoryBuilder()
        {
            _repository = new Mock<IDayHabitWriteOnlyRepository>();
        }

        public Mock<IDayHabitWriteOnlyRepository> GetMock() => _repository;

        public IDayHabitWriteOnlyRepository Build() => _repository.Object;
    }
}
