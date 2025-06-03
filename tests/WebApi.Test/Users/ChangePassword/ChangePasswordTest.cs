using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.User; 
using FluentAssertions;
using Habits.Communication.Requests.Users;
using Habits.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.ChangePassword
{
    public class ChangePasswordTest : HabitsClassFixture
    {
        private const string METHOD = "api/User/change-password";
        private readonly string _token;
        private readonly string _password;
        private readonly string _email;

        public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUser.GetToken();
            _email = webApplicationFactory.TestUser.GetEmail();
            _password = webApplicationFactory.TestUser.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;

            var loginRequest = new RequestLoginJson
            {
                Email = _email,
                Password = _password,
            };

            var response = await DoPut(METHOD, request, _token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            
            response = await DoPost("api/Login", loginRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            loginRequest.Password = request.NewPassword;

            response = await DoPost("api/Login", loginRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Password_Different_Current_Password(string culture)
        {
            var request = new RequestChangePasswordJson
            {
                Password = "!WrongPassword456",
                NewPassword = "!NewPassword123"
            };

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString(
                "PASSWORD_DIFFERENT_CURRENT_PASSWORD",
                new CultureInfo(culture)
            );

            var response = await DoPut(METHOD, request, token: _token, culture: culture);
            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);
            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
        }

        [Fact]
        public async Task Unauthorized_WithoutToken()
        {
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;

            var result = await DoPut(requestUri: METHOD, request: request, token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
