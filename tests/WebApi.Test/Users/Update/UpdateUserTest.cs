﻿using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.User;
using FluentAssertions;
using Habits.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update
{
    public class UpdateUserTest : HabitsClassFixture
    {
        private const string METHOD = "api/User";
        private readonly string _token;

        public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.TestUser.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token: _token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPut(METHOD, request, token: _token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);
            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
        }

        [Fact]
        public async Task Unauthorized_WithoutToken()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var result = await DoPut(requestUri: METHOD, request: request, token: "");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
