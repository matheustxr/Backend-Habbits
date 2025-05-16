using Habits.Domain.Entities;
using Habits.Domain.Repositories.Habits;
using Moq;

namespace CommonTestUtilities.Repositories.Habits
{
    public class HabitsUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IHabitUpdateOnlyRepository> _repository;

        public HabitsUpdateOnlyRepositoryBuilder()
        {
            _repository = new Mock<IHabitUpdateOnlyRepository>();

            _repository.Setup(repo => repo.Update(It.IsAny<Habit>()));
        }

        public HabitsUpdateOnlyRepositoryBuilder GetById(User user, Habit? habit)
        {
            if (habit is not null)
                _repository.Setup(repository => repository.GetById(user, habit.Id)).ReturnsAsync(habit);

            return this;
        }

        public IHabitUpdateOnlyRepository Build() => _repository.Object;
    }
}
