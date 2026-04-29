using System.Net.Http;

namespace PlayAPI.Tests.Stats;

public class StatsTests : PlayApiTestsBase
{
    [Test]
    public async Task GetGameStats_ReturnsGlobalStatsForGame()
    {
        var gameId = Random.Shared.NextInt64();

        using var httpClient1 = Factory.CreateClient();
        var (apiClient1, _) = CreateClientWithApi(httpClient1);
        await SetConsentAsync(httpClient1, true);
        await apiClient1.Api.Events.PostAsync(
            new PlayAPISDK.Models.RecordGameEventRequest
            {
                GameId = gameId,
                EventType = PlayAPISDK.Models.GameEventType.View,
            }
        );

        using var httpClient2 = Factory.CreateClient();
        var (apiClient2, _) = CreateClientWithApi(httpClient2);
        await SetConsentAsync(httpClient2, true);
        await apiClient2.Api.Events.PostAsync(
            new PlayAPISDK.Models.RecordGameEventRequest
            {
                GameId = gameId,
                EventType = PlayAPISDK.Models.GameEventType.View,
            }
        );

        var stats = await apiClient1.Api.GameStats.GetAsync(config =>
        {
            config.QueryParameters.GameId = gameId;
        });

        await Assert.That(stats).IsNotNull();
        await Assert.That(stats!.TotalViewCount).IsEqualTo(2);
        await Assert.That(stats.SessionCount).IsEqualTo(2);
        await Assert.That(stats.FirstVisitedAt).IsNotNull();
        await Assert.That(stats.LastVisitedAt).IsNotNull();
        await Assert
            .That(stats.FirstVisitedAt!.Value)
            .IsLessThanOrEqualTo(stats.LastVisitedAt!.Value);
    }
}
