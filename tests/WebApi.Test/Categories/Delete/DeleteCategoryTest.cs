using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.Categories;
using FluentAssertions;
using Habits.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Categories.Delete
{
    public class DeleteCategoryTest : HabitsClassFixture
    {
        private const string METHOD = "api/categories";
        private readonly string _token;
        private readonly long _categoryId;

        public DeleteCategoryTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");

            _categoryId = webApplicationFactory.TestCategory.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(requestUri: $"{METHOD}/{_categoryId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Category_Not_Found(string culture)
        {
            var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

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
            var request = RequestCategoryBuilder.Build();

            var result = await DoDelete(requestUri: $"{METHOD}/{_categoryId}", token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
