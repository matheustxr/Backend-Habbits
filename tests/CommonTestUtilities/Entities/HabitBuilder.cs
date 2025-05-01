using Bogus;
using Habits.Domain.Entities;
using Habits.Domain.Enums;

namespace CommonTestUtilities.Entities
{
    public class HabitBuilder
    {
        public static List<Habit> Collection(User user, uint count = 2)
        {
            var list = new List<Habit>();

            if (count == 0)
                count = 1;

            var habitId = 1L;

            for (int i = 0; i < count; i++)
            {
                var habit = Build(user);
                habit.Id = habitId++;
                habit.DayHabits = habit.DayHabits.Select(d => DayHabitBuilder.Build(habit.Id)).ToList();
                list.Add(habit);
            }

            return list;
        }

        public static Habit Build(User user)
        {
            return new Faker<Habit>()
                .RuleFor(h => h.Id, faker => faker.UniqueIndex)
                .RuleFor(h => h.Title, faker => faker.Lorem.Word())
                .RuleFor(h => h.Description, faker => faker.Lorem.Sentence())
                .RuleFor(h => h.WeekDays, _ => new List<WeekDays> { WeekDays.Monday, WeekDays.Wednesday })
                .RuleFor(h => h.CreatedAt, faker => faker.Date.Past())
                .RuleFor(h => h.UpdatedAt, faker => faker.Date.Recent(10))
                .RuleFor(h => h.IsActive, faker => faker.Random.Bool())
                .RuleFor(h => h.UserId, _ => user.Id)
                .RuleFor(h => h.DayHabits, faker => faker.Make(3, () => DayHabitBuilder.Build(1)))
                .RuleFor(h => h.HabitCategory, () => HabitCategoryBuilder.Build());
        }
    }
}
