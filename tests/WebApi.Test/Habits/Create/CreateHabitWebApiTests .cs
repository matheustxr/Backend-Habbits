using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Habbits.Communication.Enums;
using Habbits.Communication.Requests.Habits;

namespace WebApi.Test.Habits.Create
{
    public class CreateHabitWebApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HabitsClassFixture _fixture;

        public CreateHabitWebApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _fixture = new HabitsClassFixture(factory);
        }

        [Fact]
        public async Task Should_ReturnSuccess_When_HabitIsValid()
        {
            var request = new RequestCreateHabitJson
            {
                Title = "Read a book",
                Description = "Read for 30 minutes",
                WeekDays = new List<WeekDays> { WeekDays.Tuesday },
                IsActive = true,
                UserId = Guid.NewGuid()
            };

            var response = await _fixture.DoPostAsync("/api/habits", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_When_TitleIsEmpty()
        {
            var request = new RequestCreateHabitJson
            {
                Title = "",
                WeekDays = new List<WeekDays> { WeekDays.Friday },
                IsActive = false,
                UserId = Guid.NewGuid()
            };

            var response = await _fixture.DoPostAsync("/api/habits", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
