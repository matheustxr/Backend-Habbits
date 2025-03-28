﻿using CommonTestUtilities.Requests.Habits;
using FluentAssertions;
using Habbits.Exception;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Habits.Create;

public class CreateHabitWebApiTests : HabitsClassFixture, IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/habits";
    private readonly string _token;

    public CreateHabitWebApiTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.TestUserToken ?? throw new InvalidOperationException("Token não gerado");
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestCreateHabitJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestCreateHabitJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPost(requestUri: METHOD, request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
