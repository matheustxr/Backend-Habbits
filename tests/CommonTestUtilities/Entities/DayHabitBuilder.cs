using Bogus;
using Habbits.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class DayHabitBuilder
    {
        public static DayHabit Build(long habitId)
        {
            return new Faker<DayHabit>()
                .RuleFor(d => d.Id, _ => 1)
                .RuleFor(d => d.Date, faker => faker.Date.Past())
                .RuleFor(d => d.IsCompleted, faker => faker.Random.Bool())
                .RuleFor(d => d.HabitId, _ => habitId);
        }
    }
}
