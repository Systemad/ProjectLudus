using System.Net.Http;
using PlayAPISDK.Models;

namespace PlayAPI.Tests.Events;

public class EventTests : PlayApiTestsBase
{
    [Test]
    public async Task PostEvent_ReturnsNull_WithoutConsent()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);

        var response = await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest
            {
                GameId = Random.Shared.NextInt64(),
                EventType = GameEventType.View,
            }
        );

        await Assert.That(response).IsNull();
    }

    [Test]
    public async Task PostEvent_ReturnsEvent_WithConsent()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);

        await SetConsentAsync(httpClient, true);

        var response = await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest
            {
                GameId = Random.Shared.NextInt64(),
                EventType = GameEventType.View,
            }
        );

        await Assert.That(response).IsNotNull();
    }

    [Test]
    public async Task PostEvent_CreatesSessionId()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);

        await SetConsentAsync(httpClient, true);

        var response = await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest
            {
                GameId = Random.Shared.NextInt64(),
                EventType = GameEventType.View,
            }
        );

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.SessionId).IsNotEqualTo(Guid.Empty);
    }
}
