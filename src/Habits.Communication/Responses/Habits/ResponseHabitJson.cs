using Habits.Communication.Enums;

namespace Habits.Communication.Responses.Habits
{
    public class ResponseHabitJson
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<WeekDays> WeekDays { get; set; } = new();
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
