namespace Habits.Communication.Responses.Summary
{
    public class ResponseSummaryHabitJson
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
