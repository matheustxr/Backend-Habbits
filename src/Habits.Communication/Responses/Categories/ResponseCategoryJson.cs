namespace Habits.Communication.Responses.Categories
{
    public class ResponseCategoryJson
    {
        public required string Category { get; set; }
        public string? HexColor { get; set; } = "#FFFFFF";
    }
}
