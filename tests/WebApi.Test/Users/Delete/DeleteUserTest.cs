using System.Net;
using FluentAssertions;

namespace WebApi.Test.Users.Delete
{
    public class DeleteUserTest : HabitsClassFixture
    {
        private const string METHOD = "api/User";
        private readonly string _token;

        public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUser.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(METHOD, _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Unauthorized_WithoutToken()
        {
            var result = await DoGet(requestUri: METHOD, token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
