namespace Habits.Communication.Responses.Summary
{
    public class ResponseSummaryJson
    {
        public DateOnly Date { get; set; }
        public int Completed { get; set; }
        public int Amount { get; set; }
    }
}
