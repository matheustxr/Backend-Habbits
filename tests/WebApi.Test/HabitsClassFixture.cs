using System.Net.Http.Headers;
using System.Net.Http.Json;
using Habits.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class HabitsClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    public readonly HabbitsDbContext DbContext;

    public HabitsClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();

        DbContext = webApplicationFactory.Services.GetRequiredService<HabbitsDbContext>();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }

    protected async Task<HttpResponseMessage> DoPost(
        string requestUri,
        object request,
        string token = "",
        string culture = "en")
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = JsonContent.Create(request)
        };

        if (!string.IsNullOrWhiteSpace(token))
        {
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        httpRequest.Headers.AcceptLanguage.Clear();
        httpRequest.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));

        return await _httpClient.SendAsync(httpRequest);
    }

    protected async Task<HttpResponseMessage> DoPut(
        string requestUri,
        object request,
        string token,
        string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);
        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> DoGet(
        string requestUri,
        string token,
        string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        return await _httpClient.GetAsync(requestUri);
    }

    protected async Task<HttpResponseMessage> DoDelete(
        string requestUri,
        string token,
        string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);
        return await _httpClient.DeleteAsync(requestUri);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}
