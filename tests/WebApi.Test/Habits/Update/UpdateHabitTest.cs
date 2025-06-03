using CommonTestUtilities.Requests.Habits;
using FluentAssertions;
using Habits.Exception;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Habits.Update
{
    public class UpdateHabitTest : HabitsClassFixture, IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/habits";
        private readonly string _token;
        private readonly long _habitId;

        public UpdateHabitTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");

            _habitId = webApplicationFactory.TestHabit.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestHabitJsonBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/{_habitId}", request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Title_Empty(string culture)
        {
            var request = RequestHabitJsonBuilder.Build();

            request.Title = string.Empty;

            var result = await DoPut(requestUri: $"{METHOD}/{_habitId}", request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Habit_Not_Found(string culture)
        {
            var request = RequestHabitJsonBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, culture: culture);

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
            var request = RequestHabitJsonBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/{_habitId}", request, token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
