using Bogus;
using Habits.Communication.Enums; 
using Habits.Communication.Requests.Habits;

namespace CommonTestUtilities.Requests.Habits
{
    public class RequestHabitJsonBuilder
    {
        public static RequestHabitJson Build()
        {
            var faker = new Faker();

            var habit = new RequestHabitJson
            {
                Title = faker.Lorem.Sentence(),
                Description = faker.Lorem.Paragraph(),
                WeekDays = GenerateRandomWeekDays(),
                IsActive = faker.Random.Bool(),
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
