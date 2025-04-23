using Habits.Communication.Enums;

namespace Habits.Communication.Responses.Habits
{
    public class ResponseShortHabitJson
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<WeekDays> WeekDays { get; set; } = new();
        public bool IsActive { get; set; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
