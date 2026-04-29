using System.Net.Http;
using PlayAPISDK.Models;

namespace PlayAPI.Tests.Metrics;

public class MetricsTests : PlayApiTestsBase
{
    [Test]
    public async Task GetGameMetric_ReturnsStatsForSession()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);
        var gameId = Random.Shared.NextInt64();

        await SetConsentAsync(httpClient, true);

        var eventResponse = await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest { GameId = gameId, EventType = GameEventType.View }
        );

        await Assert.That(eventResponse).IsNotNull();
        SetSessionCookie(httpClient, eventResponse!.SessionId.GetValueOrDefault());

        var metric = await apiClient.Api.GameMetrics.GetAsync(config =>
        {
            config.QueryParameters.GameId = gameId;
        });

        await Assert.That(metric).IsNotNull();
        await Assert.That(metric!.GameId).IsEqualTo(gameId);
        await Assert.That(metric.SessionId).IsEqualTo(eventResponse.SessionId);
        await Assert.That(metric.ViewCount).IsEqualTo(1);
    }

    [Test]
    public async Task GetGameMetric_CountIncrements_ForSameSession()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);
        var gameId = Random.Shared.NextInt64();

        await SetConsentAsync(httpClient, true);

        var firstResponse = await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest { GameId = gameId, EventType = GameEventType.View }
        );
        await Assert.That(firstResponse).IsNotNull();
        SetSessionCookie(httpClient, firstResponse!.SessionId.GetValueOrDefault());

        await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest { GameId = gameId, EventType = GameEventType.View }
        );

        var metric = await apiClient.Api.GameMetrics.GetAsync(config =>
        {
            config.QueryParameters.GameId = gameId;
        });

        await Assert.That(metric).IsNotNull();
        await Assert.That(metric!.ViewCount).IsEqualTo(2);
    }
}
