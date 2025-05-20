namespace Habits.Communication.Requests.Categories
{
    public class RequestCategoryJson
    {
        public required string Category { get; set; }
        public string? HexColor { get; set; } = "#FFFFFF";
    }
}
