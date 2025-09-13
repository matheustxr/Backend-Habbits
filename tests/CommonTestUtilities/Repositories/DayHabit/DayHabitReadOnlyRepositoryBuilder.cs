using Habits.Domain.Repositories.DayHabits;
using Moq;

namespace CommonTestUtilities.Repositories.DayHabit
{
    public class DayHabitReadOnlyRepositoryBuilder
    {
        private readonly Mock<IDayHabitReadOnlyRepository> _repository;

        public DayHabitReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IDayHabitReadOnlyRepository>();
        }

        public DayHabitReadOnlyRepositoryBuilder GetHabitsForDate(List<(long habitId, string title, string? categoryName, bool isCompleted, DateTime createdAt, DateTime? updatedAt)> habits)
        {
            _repository.Setup(repo => repo.GetHabitsForDateAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>()))
                       .ReturnsAsync(habits);
            return this;
        }

        public DayHabitReadOnlyRepositoryBuilder GetMonthlySummary(Dictionary<DateOnly, (int possible, int completed)> summary)
        {
            _repository.Setup(repo => repo.GetDateRangeSummaryAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                       .ReturnsAsync(summary);
            return this;
        }

        public IDayHabitReadOnlyRepository Build() => _repository.Object;
    }
}
