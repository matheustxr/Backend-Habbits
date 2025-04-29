using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebApi.Test.Habits.GetAll
{
    public class GetAllHabitsTest : HabitsClassFixture, IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/habits";
        private readonly string _token;

        public GetAllHabitsTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoGet(requestUri: METHOD, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("habits").EnumerateArray().Should().NotBeNullOrEmpty();
        }
    }
}
