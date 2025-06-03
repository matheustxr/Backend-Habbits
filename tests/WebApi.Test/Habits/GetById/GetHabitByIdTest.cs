using System.Globalization;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using Habits.Domain.Enums;
using Habits.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Habits.GetById
{
    public class GetHabitByIdTest : HabitsClassFixture, IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/habits";
        private readonly string _token;
        private readonly long _habitId;

        public GetHabitByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");
            _habitId = webApplicationFactory.TestHabit.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoGet(requestUri: $"{METHOD}/{_habitId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);
            
            response.RootElement.GetProperty("id").GetInt64().Should().Be(_habitId);
            response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
            response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
            response.RootElement.GetProperty("createdAt").GetDateTime().Should().NotBeAfter(DateTime.Today);

            response.RootElement.TryGetProperty("weekDays", out var weekDaysElement)
                .Should().BeTrue("A resposta JSON deve conter a propriedade 'weekDays'");

            var weekDaysArray = weekDaysElement.EnumerateArray();

            weekDaysArray.Should().NotBeNullOrEmpty("weekDays deve conter pelo menos um dia da semana");

            foreach (var dayElement in weekDaysArray)
            {
                var dayString = dayElement.GetString();
                dayString.Should().NotBeNullOrWhiteSpace("cada item em weekDays deve ser uma string válida");

                Enum.TryParse(typeof(WeekDays), dayString, ignoreCase: true, out var parsed)
                    .Should().BeTrue($"'{dayString}' deve ser um nome válido do enum WeekDays (case-insensitive)");
            }
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
            var result = await DoGet(requestUri: $"{METHOD}/{_habitId}", token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
