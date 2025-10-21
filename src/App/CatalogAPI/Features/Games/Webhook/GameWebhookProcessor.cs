using CatalogAPI.Data.Features.Games;
using IGDB.Lib;
using IGDB.Lib.Webhooks;

namespace CatalogAPI.Features.Games.Webhook;

public class GameWebhookProcessor(
    GameDatabaseService databaseService,
    ApiClient apiClient,
    WebhookApiClient webhookApiClient
)
{
    private GameDatabaseService _databaseService = databaseService;
    private ApiClient _apiClient = apiClient;
    private WebhookApiClient _webhookApiClient = webhookApiClient;

    public async Task ProcessWebhookEventAsync(long gameId, WebhookMethod method)
    {
        switch (method)
        {
            case WebhookMethod.CREATE:
            case WebhookMethod.UPDATE:
                await UpdateGameAsync(gameId);
                break;
            case WebhookMethod.DELETE:
                await DeleteGameAsync(gameId);
                break;
        }
    }

    public async Task ProcessWebhookEventAsync(long gameId)
    {
        await DeleteGameAsync(gameId);
    }

    private async Task UpdateGameAsync(long id)
    {
        var game = await _apiClient.FetchGameAsync(id);
        await _databaseService.UpdateGameAsync(new GameEntity());
    }

    private async Task DeleteGameAsync(long id)
    {
        await _databaseService.DeleteGameAsync(id);
    }

    public async Task SubscribeWebhookEndpointAsync(string endpoint)
    {
        try
        {
            await _webhookApiClient.SubscribeWebhooksAsync(endpoint);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<WebhookResponse>> FetchActiveWebhooksAsync()
    {
        var allWebhooks = await _webhookApiClient.GetWebhooksAsync();
        return allWebhooks.Where(w => w.Active).ToList();
    }
}
