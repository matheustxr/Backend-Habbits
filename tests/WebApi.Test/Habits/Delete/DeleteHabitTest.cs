using System.Globalization;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using Habits.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Habits.Delete
{
    public class DeleteHabitTest : HabitsClassFixture, IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/habits";
        private readonly string _token;
        private readonly long _habitId;

        public DeleteHabitTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");
            _habitId = webApplicationFactory.TestHabit.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(requestUri: $"{METHOD}/{_habitId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);

            result = await DoGet(requestUri: $"{METHOD}/{_habitId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Habit_Not_Found(string culture)
        {
            var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("HABIT_NOT_FOUND", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Fact]
        public async Task Unauthorized_WithoutToken()
        {
            var result = await DoDelete(requestUri: $"{METHOD}/{_habitId}", token:"");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
