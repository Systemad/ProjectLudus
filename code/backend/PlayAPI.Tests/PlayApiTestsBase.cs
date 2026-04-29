using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using PlayAPI.Tests.Setup;
using PlayAPISDK;
using PlayAPISDK.Models;
using TUnit.AspNetCore;

namespace PlayAPI.Tests;

public abstract class PlayApiTestsBase : WebApplicationTest<PlayApiWebApplicationFactory, Program>
{
    protected static (ApiClient ApiClient, HttpClient HttpClient) CreateClientWithApi(
        HttpClient httpClient
    )
    {
        var apiClient = PlayApiWebApplicationFactory.CreateApiClient(httpClient);
        return (apiClient, httpClient);
    }

    protected static async Task SetConsentAsync(HttpClient httpClient, bool consent)
    {
        var response = await httpClient.PostAsJsonAsync(
            "/api/cookies/consent",
            new { Consent = consent }
        );

        response.EnsureSuccessStatusCode();
        UpdateCookiesFromResponse(httpClient, response);

        if (!consent)
        {
            ClearCookies(httpClient);
        }
    }

    protected static async Task RemoveConsentAsync(HttpClient httpClient)
    {
        var response = await httpClient.DeleteAsync("/api/cookies/consent");
        response.EnsureSuccessStatusCode();
        ClearCookies(httpClient);
    }

    protected static async Task<CookieConsentResponse> GetConsentStatusAsync(ApiClient apiClient)
    {
        using var stream = await apiClient.Api.Cookies.Consent.GetAsync();
        return JsonSerializer.Deserialize<CookieConsentResponse>(
            stream!,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;
    }

    protected static void UpdateCookiesFromResponse(
        HttpClient httpClient,
        HttpResponseMessage response
    )
    {
        if (!response.Headers.TryGetValues("Set-Cookie", out var setCookieValues))
        {
            return;
        }

        var cookiePairs = setCookieValues
            .Select(value => value.Split(';', 2)[0].Trim())
            .Where(pair => pair.Length > 0)
            .ToArray();

        if (cookiePairs.Length == 0)
        {
            return;
        }

        if (httpClient.DefaultRequestHeaders.Contains("Cookie"))
        {
            httpClient.DefaultRequestHeaders.Remove("Cookie");
        }

        httpClient.DefaultRequestHeaders.Add("Cookie", string.Join("; ", cookiePairs));
    }

    protected static void ClearCookies(HttpClient httpClient)
    {
        if (httpClient.DefaultRequestHeaders.Contains("Cookie"))
        {
            httpClient.DefaultRequestHeaders.Remove("Cookie");
        }
    }

    protected static void SetSessionCookie(HttpClient httpClient, Guid sessionId)
    {
        var cookieValue = $"AnalyticsSessionId={sessionId:D}";
        if (httpClient.DefaultRequestHeaders.TryGetValues("Cookie", out var existingCookies))
        {
            var cookies = existingCookies
                .SelectMany(value => value.Split(';', StringSplitOptions.RemoveEmptyEntries))
                .Select(value => value.Trim())
                .Where(value =>
                    !value.StartsWith("AnalyticsSessionId=", StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            cookies.Add(cookieValue);
            httpClient.DefaultRequestHeaders.Remove("Cookie");
            httpClient.DefaultRequestHeaders.Add("Cookie", string.Join("; ", cookies));
            return;
        }

        httpClient.DefaultRequestHeaders.Add("Cookie", cookieValue);
    }

    protected sealed record CookieConsentResponse(bool Consent);
}
