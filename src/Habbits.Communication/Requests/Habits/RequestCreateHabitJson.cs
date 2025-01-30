using Habbits.Communication.Enums;

namespace Habbits.Communication.Requests.Habits;

public class RequestCreateHabitJson
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public List<WeekDays> WeekDays { get; set; } = new();
    public bool IsActive { get; set; } = true;
}
