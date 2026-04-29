using System.Net.Http;
using PlayAPISDK.Models;

namespace PlayAPI.Tests.Flows;

public class UserFlowTests : PlayApiTestsBase
{
    [Test]
    public async Task UserFlow_ConsentEnablesTracking()
    {
        using var httpClient = Factory.CreateClient();
        var (apiClient, _) = CreateClientWithApi(httpClient);
        var gameId = Random.Shared.NextInt64();

        var firstResponse = await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest { GameId = gameId, EventType = GameEventType.View }
        );

        await Assert.That(firstResponse).IsNull();

        await SetConsentAsync(httpClient, true);

        var secondResponse = await apiClient.Api.Events.PostAsync(
            new RecordGameEventRequest { GameId = gameId, EventType = GameEventType.View }
        );

        await Assert.That(secondResponse).IsNotNull();
        SetSessionCookie(httpClient, secondResponse!.SessionId.GetValueOrDefault());

        var metric = await apiClient.Api.GameMetrics.GetAsync(config =>
        {
            config.QueryParameters.GameId = gameId;
        });

        await Assert.That(metric).IsNotNull();
    }
}
