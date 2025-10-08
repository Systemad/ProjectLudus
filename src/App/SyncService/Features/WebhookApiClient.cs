using Shared.Features.Webhooks;

namespace SyncService.Features;

public class WebhookApiClient(HttpClient httpClient)
{
    public async Task SubscribeWebhooksAsync(string endpointUrl)
    {
        await Task.WhenAll(
            SubscribeWebhookAsync(endpointUrl, WebhookMethod.CREATE),
            SubscribeWebhookAsync(endpointUrl, WebhookMethod.UPDATE),
            SubscribeWebhookAsync(endpointUrl, WebhookMethod.DELETE)
        );
    }

    public async Task UnSubscribeWebhookAsync(long webhookId)
    {
        var response = await httpClient.DeleteAsync($"webhooks/{webhookId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<WebhookResponse>> GetWebhooksAsync()
    {
        var response = await httpClient.GetAsync("webhooks");
        response.EnsureSuccessStatusCode();

        var webhooks = await response.Content.ReadFromJsonAsync<List<WebhookResponse>>();
        if (webhooks == null)
            throw new Exception("Failed to fetch webhooks!");

        return webhooks;
    }

    private async Task<WebhookResponse> SubscribeWebhookAsync(
        string endpointUrl,
        WebhookMethod method
    )
    {
        var formContent = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["url"] = endpointUrl,
                ["secret"] = Guid.NewGuid().ToString(),
                ["method"] = method.ToString(),
            }
        );

        var response = await httpClient.PostAsync("webhooks", formContent);
        response.EnsureSuccessStatusCode();

        var webhook = await response.Content.ReadFromJsonAsync<WebhookResponse>();
        if (webhook == null)
            throw new Exception("Failed to subscribe webhook!");

        return webhook;
    }
}
