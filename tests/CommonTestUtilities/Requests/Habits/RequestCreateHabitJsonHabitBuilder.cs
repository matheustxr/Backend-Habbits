using Bogus;
using Habbits.Communication.Requests.Habits;
using Habbits.Communication.Enums;  // Usando o namespace correto para WeekDays
using System;
using System.Collections.Generic;

namespace CommonTestUtilities.Requests.Habits
{
    public class RequestCreateHabitJsonHabitBuilder
    {
        public static RequestCreateHabitJson Build()
        {
            var faker = new Faker();

            var habit = new RequestCreateHabitJson
            {
                Title = faker.Lorem.Sentence(),  
                Description = faker.Lorem.Paragraph(),  
                WeekDays = GenerateRandomWeekDays(), 
                IsActive = faker.Random.Bool(),  
                UserId = Guid.NewGuid()  
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
