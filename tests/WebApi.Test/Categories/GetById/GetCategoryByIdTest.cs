using System.Globalization;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using Habits.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Categories.GetById
{
    public class GetCategoryByIdTest : HabitsClassFixture
    {
        private const string METHOD = "api/categories";
        private readonly string _token;
        private readonly long _categoryId;

        public GetCategoryByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");
            _categoryId = webApplicationFactory.TestCategory.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoGet(requestUri: $"{METHOD}/{_categoryId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("id").GetInt64().Should().Be(_categoryId);
            response.RootElement.GetProperty("category").GetString().Should().NotBeNullOrWhiteSpace();
            response.RootElement.GetProperty("hexColor").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Category_Not_Found(string culture)
        {
            var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("CATEGORY_NOT_FOUND", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Fact]
        public async Task Unauthorized_WithoutToken()
        {
            var result = await DoGet(requestUri: $"{METHOD}/{_categoryId}", token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
