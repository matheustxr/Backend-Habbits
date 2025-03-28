using Bogus;
using Habbits.Communication.Enums; 
using Habbits.Communication.Requests.Habits;

namespace CommonTestUtilities.Requests.Habits
{
    public class RequestCreateHabitJsonBuilder
    {
        private static readonly Guid FixedUserId = Guid.NewGuid();

        public static RequestCreateHabitJson Build()
        {
            var faker = new Faker();

            var habit = new RequestCreateHabitJson
            {
                Title = faker.Lorem.Sentence(),
                Description = faker.Lorem.Paragraph(),
                WeekDays = GenerateRandomWeekDays(),
                IsActive = faker.Random.Bool(),
                UserId = FixedUserId
            };

            return habit;
        }

        
        private static List<WeekDays> GenerateRandomWeekDays()
        {
            var random = new Random();
            var daysOfWeek = (WeekDays[])Enum.GetValues(typeof(WeekDays));  
            var numberOfDays = random.Next(1, daysOfWeek.Length + 1); 

            var selectedDays = new List<WeekDays>();
            while (selectedDays.Count < numberOfDays)
            {
                var day = daysOfWeek[random.Next(daysOfWeek.Length)];
                if (!selectedDays.Contains(day))
                {
                    selectedDays.Add(day);
                }
            }

            return selectedDays;
        }
    }
}
