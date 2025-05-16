using Habits.Domain.Entities;
using Habits.Domain.Repositories.Habits;
using Moq;

namespace CommonTestUtilities.Repositories.Habits
{
    public class HabitsReadOnlyRepositoryBuilder
    {
        private readonly Mock<IHabitReadOnlyRepository> _repository;

        public HabitsReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IHabitReadOnlyRepository>();
        }

        public HabitsReadOnlyRepositoryBuilder GetAll(User user, List<Habit> habits)
        {
            _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(habits);

            return this;
        }

        public HabitsReadOnlyRepositoryBuilder GetById(User user, Habit habit)
        {
            _repository
                .Setup(repository => repository.GetById(
                    It.Is<User>(u => u.Id == user.Id),
                    It.Is<long>(id => id == habit.Id)))
                .ReturnsAsync(habit);

            return this;
        }

        public IHabitReadOnlyRepository Build() => _repository.Object;
    }
}
