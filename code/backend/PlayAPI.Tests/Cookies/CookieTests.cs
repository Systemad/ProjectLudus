using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using PlayAPISDK.Models;

namespace PlayAPI.Tests.Cookies;

public class CookieTests : PlayApiTestsBase
{
    [Test]
    public async Task DefaultConsent_IsFalse()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);

        var consentStatus = await GetConsentStatusAsync(apiClient);

        await Assert.That(consentStatus).IsNotNull();
        await Assert.That(consentStatus!.Consent).IsFalse();
    }

    [Test]
    public async Task SetConsent_ToTrue_PersistsTrue()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);

        await SetConsentAsync(httpClient, true);
        var consentStatus = await GetConsentStatusAsync(apiClient);

        await Assert.That(consentStatus).IsNotNull();
        await Assert.That(consentStatus!.Consent).IsTrue();
    }

    [Test]
    public async Task RemoveConsent_ResetsToFalse()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);

        await SetConsentAsync(httpClient, true);
        await RemoveConsentAsync(httpClient);

        var consentStatus = await GetConsentStatusAsync(apiClient);

        await Assert.That(consentStatus).IsNotNull();
        await Assert.That(consentStatus!.Consent).IsFalse();
    }
}
