using CommonTestUtilities.Requests.User;
using FluentAssertions;
using Habbits.Communication.Requests.Users;
using Habbits.Exception;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login
{
    public class DoLoginTest : HabitsClassFixture
    {
        private const string METHOD = "api/Login";

        private readonly string _email;
        private readonly string _name;
        private readonly string _password;

        public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _email = webApplicationFactory.TestUser.GetEmail();
            _name = webApplicationFactory.TestUser.GetName();
            _password = webApplicationFactory.TestUser.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            // Arrange
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            // Act
            var response = await DoPost(requestUri: METHOD, request: request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
            responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Login_Invalid(string culture)
        {
            // Arrange
            var request = RequestLoginJsonBuilder.Build();

            // Act
            var response = await DoPost(requestUri: METHOD, request: request, culture: culture);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);
            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager
                .GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
        }
    }
}
