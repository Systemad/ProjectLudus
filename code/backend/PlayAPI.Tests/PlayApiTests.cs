using PlayAPI.Tests.Setup;
using PlayAPISDK.Models;
using TUnit.AspNetCore;

namespace PlayAPI.Tests;

public class PlayApiTests : WebApplicationTest<PlayApiWebApplicationFactory, Program>
{
    [Test]
    public async Task PostEvent_WithValidViewEvent_ReturnsOkAndCreatesSessionStats()
    {
        var apiClient = PlayApiWebApplicationFactory.CreateApiClient(Factory.CreateClient());
        var sessionId = Guid.NewGuid().ToString();
        var gameId = Random.Shared.NextInt64();

        var response = await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest
            {
                GameId = gameId,
                EventType = GameEventType.View,
                SessionId = sessionId,
            }
        );

        await Assert.That(response).IsNotNull();
    }

    [Test]
    public async Task GetGameMetric_ReturnsStatsForSession()
    {
        var apiClient = PlayApiWebApplicationFactory.CreateApiClient(Factory.CreateClient());
        var sessionId = Guid.NewGuid().ToString();
        var gameId = Random.Shared.NextInt64();
        
        await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest
            {
                GameId = gameId,
                EventType = GameEventType.View,
                SessionId = sessionId,
            }
        );

        var gameMetric = await apiClient.Api.GameMetrics.GetAsync(config =>
        {
            config.QueryParameters.GameId = gameId;
            config.QueryParameters.SessionId = Guid.Parse(sessionId);
        });

        await Assert.That(gameMetric).IsNotNull();
        await Assert.That(gameMetric!.GameId).IsEqualTo(gameId);
        await Assert.That(gameMetric.SessionId).IsEqualTo(Guid.Parse(sessionId));
        await Assert.That(gameMetric.ViewCount).IsEqualTo(1);
    }

    [Test]
    public async Task GetGameStats_ReturnsGlobalStatsForGame()
    {
        var apiClient = PlayApiWebApplicationFactory.CreateApiClient(Factory.CreateClient());
        var firstSession = Guid.NewGuid().ToString();
        var secondSession = Guid.NewGuid().ToString();

        var gameId = Random.Shared.NextInt64();
        
        await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest
            {
                GameId = gameId,
                EventType = GameEventType.View,
                SessionId = firstSession,
            }
        );

        await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest
            {
                GameId = gameId,
                EventType = GameEventType.View,
                SessionId = secondSession,
            }
        );

        var gameStats = await apiClient.Api.GameStats.GetAsync(config =>
        {
            config.QueryParameters.GameId = gameId;
        });

        await Assert.That(gameStats).IsNotNull();
        await Assert.That(gameStats!.GameId).IsEqualTo(gameId);
        await Assert.That(gameStats.TotalViewCount).IsEqualTo(2);
        await Assert.That(gameStats.SessionCount).IsEqualTo(2);
        await Assert.That(gameStats.FirstVisitedAt).IsNotNull();
        await Assert.That(gameStats.LastVisitedAt).IsNotNull();
        await Assert
            .That(gameStats.FirstVisitedAt!.Value)
            .IsLessThanOrEqualTo(gameStats.LastVisitedAt!.Value);
    }
}
