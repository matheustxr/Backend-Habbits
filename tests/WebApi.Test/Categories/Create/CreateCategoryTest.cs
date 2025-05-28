using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.Categories;
using FluentAssertions;
using Habits.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Categories.Create
{
    public class CreateCategoryTest : HabitsClassFixture, IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/categories";
        private readonly string _token;

        public CreateCategoryTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestCategoryBuilder.Build();

            var result = await DoPost(requestUri: METHOD, request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.Created);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("category").GetString().Should().Be(request.Category);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Title_Empty(string culture)
        {
            var request = RequestCategoryBuilder.Build();
            request.Category = string.Empty;

            var result = await DoPost(requestUri: METHOD, request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
