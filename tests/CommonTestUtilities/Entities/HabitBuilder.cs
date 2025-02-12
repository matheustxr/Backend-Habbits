using Bogus;
using Habbits.Domain.Entities;

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
                list.Add(habit);
            }

            return list;
        }

        public static Habit Build(User user)
        {
            return new Faker<Habit>()
                .RuleFor(h => h.Id, _ => 1)
                .RuleFor(h => h.Title, faker => faker.Lorem.Word())
                .RuleFor(h => h.Description, faker => faker.Lorem.Sentence())
                .RuleFor(h => h.CreatedAt, faker => faker.Date.Past())
                .RuleFor(h => h.UpdatedAt, faker => faker.Date.Recent(10))
                .RuleFor(h => h.IsActive, faker => faker.Random.Bool())
                .RuleFor(h => h.UserId, _ => user.Id)
                .RuleFor(h => h.DayHabits, faker => faker.Make(3, () => DayHabitBuilder.Build(1))) // 3 hábitos diários aleatórios
                .RuleFor(h => h.HabitCategory, () => HabitCategoryBuilder.Build()); // Adiciona uma categoria aleatória
        }
    }
}
