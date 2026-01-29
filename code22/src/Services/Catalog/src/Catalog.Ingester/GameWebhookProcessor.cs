using Catalog.Ingester.Webhooks;

namespace Catalog.Ingester;

public class GameWebhookProcessor(
    WebhookClient webhookClient
)
{
    private WebhookClient _webhookClient = webhookClient;

    public async Task ProcessWebhookEventAsync(long gameId, WebhookMethod method)
    {
        switch (method)
        {
            case WebhookMethod.CREATE:
            case WebhookMethod.UPDATE:
            case WebhookMethod.DELETE:
                break;
        }
    }

    public async Task ProcessWebhookEventAsync(long gameId)
    {
    }



    public async Task SubscribeWebhookEndpointAsync(string endpoint)
    {
        try
        {
            await _webhookClient.SubscribeWebhooksAsync(endpoint);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<WebhookResponse>> FetchActiveWebhooksAsync()
    {
        var allWebhooks = await _webhookClient.GetWebhooksAsync();
        return allWebhooks.Where(w => w.Active).ToList();
    }
}
