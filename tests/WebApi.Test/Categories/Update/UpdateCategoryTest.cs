using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.Categories;
using FluentAssertions;
using Habits.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Categories.Update
{
    public class UpdateCategoryTest : HabitsClassFixture
    {
        private const string METHOD = "api/categories";
        private readonly string _token;
        private readonly long _categoryId;

        public UpdateCategoryTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");

            _categoryId = webApplicationFactory.TestCategory.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestCategoryBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/{_categoryId}", request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Title_Empty(string culture)
        {
            var request = RequestCategoryBuilder.Build();

            request.Category = string.Empty;

            var result = await DoPut(requestUri: $"{METHOD}/{_categoryId}", request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Category_Not_Found(string culture)
        {
            var request = RequestCategoryBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, culture: culture);

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

            var result = await DoPut(requestUri: $"{METHOD}/{_categoryId}", request, token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
