using Habits.Domain.Repositories.Habits;
using Moq;

namespace CommonTestUtilities.Repositories.Habits
{
    public class HabitsWriteOnlyRepositoryBuilder
    {
        private readonly Mock<IHabitWriteOnlyRepository> _repository;

        public HabitsWriteOnlyRepositoryBuilder()
        {
            _repository = new Mock<IHabitWriteOnlyRepository>();
        }

        public IHabitWriteOnlyRepository Build() => _repository.Object;
    }
}
