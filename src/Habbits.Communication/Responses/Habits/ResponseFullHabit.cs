using Habbits.Communication.Enums;

namespace Habbits.Communication.Responses.Habbits
{
    public class ResponseFullHabit
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<WeekDays> WeekDays { get; set; } = new();
        public bool IsActive { get; set; }
    }
}
