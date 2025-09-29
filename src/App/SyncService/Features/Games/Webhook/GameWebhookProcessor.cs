using Shared.Features.Webhooks;
using SyncService.Utilities;

namespace SyncService.Features.Games.Webhook;

// TODO: MAKE THIS MORE GENERIC, AND INCLUDE ALL FIELDS
public class GameWebhookProcessor(
    GameDatabaseService databaseService,
    WebhookDatabaseService webhookDatabaseService,
    ApiClient apiClient,
    WebhookApiClient webhookApiClient)
{
    private WebhookDatabaseService _webhookDatabaseService = webhookDatabaseService;
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

        await _webhookDatabaseService.UpdateWebhookStatusAsync(method);
    }

    public async Task ProcessWebhookEventAsync(long gameId)
    {
        await DeleteGameAsync(gameId);
        await _webhookDatabaseService.UpdateWebhookStatusAsync(WebhookMethod.DELETE);
    }

    private async Task UpdateGameAsync(long id)
    {
        var game = await _apiClient.FetchGameAsync(id);
        var dto = game.ToEntity();
        await _databaseService.UpdateGameAsync(dto);
    }

    private async Task DeleteGameAsync(long id)
    {
        await _databaseService.DeleteGameAsync(id);
    }

    public async Task SubscribeWebhookEndpointAsync(string endpoint)
    {
        await UnSubscribeAllWebhooksAsync();
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

    public async Task UnSubscribeAllWebhooksAsync()
    {
        var webhooks = await _webhookDatabaseService.GetWebhooksAsync();
        foreach (var webhook in webhooks)
        {
            await _webhookApiClient.UnSubscribeWebhookAsync(webhook.Id);
        }
    }

    public async Task<List<WebhookResponse>> FetchActiveWebhooksAsync()
    {
        var allWebhooks = await _webhookApiClient.GetWebhooksAsync();
        return allWebhooks.Where(w => w.Active).ToList();
    }
}