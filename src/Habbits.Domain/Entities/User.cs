namespace Habbits.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public List<int> WeekDays { get; set; } = new();
        
    public bool IsActive { get; set; } = true;
}
