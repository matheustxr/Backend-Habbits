using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Categories.GetAll
{
    public class GetAllCategoriesTest : HabitsClassFixture
    {
        private const string METHOD = "api/categories";
        private readonly string _token;

        public GetAllCategoriesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
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

            response.RootElement.GetProperty("categories").EnumerateArray().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Unauthorized_WithoutToken()
        {
            var result = await DoGet(requestUri: METHOD, token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
