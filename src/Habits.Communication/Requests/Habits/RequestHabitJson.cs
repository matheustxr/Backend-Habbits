using System.Text.Json.Serialization;
using Habits.Communication.Enums;

namespace Habits.Communication.Requests.Habits;

public class RequestHabitJson
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public List<WeekDays> WeekDays { get; set; } = new();
    public bool IsActive { get; set; } = true;
    public long? CategoryId { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
}
