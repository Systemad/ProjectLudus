using System.Text.Json;
using Microsoft.Extensions.Options;

public class WebhookManager(
    HttpClient http,
    IOptions<IGDBOptions> options,
    ILogger<WebhookManager> logger
)
{
    public async Task<List<WebhookResponse>> GetAll()
    {
        var resp = await http.GetAsync("webhooks");
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync();
            logger.LogInformation(
                "GetAll: HTTP {(int)resp.StatusCode} — {Body}",
                (int)resp.StatusCode,
                body
            );
            return [];
        }
        return await resp.Content.ReadFromJsonAsync<List<WebhookResponse>>() ?? [];
    }

    public async Task<WebhookResponse?> Subscribe(string endpoint)
    {
        var body = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["url"] = options.Value.WEBHOOK_URL,
                ["secret"] = options.Value.WEBHOOK_SECRET,
                ["method"] = "delete",
            }
        );
        var resp = await http.PostAsync($"{endpoint}/webhooks/", body);
        if (!resp.IsSuccessStatusCode)
        {
            var err = await resp.Content.ReadAsStringAsync();
            logger.LogInformation(
                "Subscribe {Endpoint}: HTTP {(int)resp.StatusCode} — {Error}",
                endpoint,
                (int)resp.StatusCode,
                err
            );
            return null;
        }
        var list = await resp.Content.ReadFromJsonAsync<List<WebhookResponse>>();
        return list?.FirstOrDefault();
    }

    public async Task<bool> Unsubscribe(long webhookId)
    {
        var resp = await http.DeleteAsync($"webhooks/{webhookId}");
        if (!resp.IsSuccessStatusCode)
        {
            logger.LogInformation(
                "Unsubscribe {Id}: HTTP {(int)resp.StatusCode}",
                webhookId,
                (int)resp.StatusCode
            );
            return false;
        }
        return true;
    }

    public EndpointConfig GetConfig()
    {
        var json = File.ReadAllText("../endpoints.json");
        return JsonSerializer.Deserialize<EndpointConfig>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;
    }

    public async Task UnsubscribeAllAsync()
    {
        var existing = await GetAll();
        foreach (var wh in existing)
            await Unsubscribe(wh.Id);
        logger.LogInformation("Unsubscribed {Count} existing.", existing.Count);
    }

    public async Task SubscribeAllASync()
    {
        var cfg = GetConfig();
        foreach (var ep in cfg.Default)
        {
            var result = await Subscribe(ep);
            await Task.Delay(250);
            if (result is not null)
                logger.LogInformation("Subscribed {Endpoint} (id={Id})", ep, result.Id);
        }
    }

    public async Task SyncAsync()
    {
        logger.LogInformation(
            "IGDB: Client-ID={Id}, Token={Token}, URL={Url}",
            options.Value.CLIENT_ID,
            options.Value.ACCESS_TOKEN,
            options.Value.WEBHOOK_URL
        );

        await UnsubscribeAllAsync();
        logger.LogInformation("Clearing and re-registering webhooks...");
        await SubscribeAllASync();
    }

    public async Task<string> Test(string endpoint, int webhookId, int entityId)
    {
        var resp = await http.PostAsync(
            $"{endpoint}/webhooks/test/{webhookId}?entityId={entityId}",
            null
        );
        var body = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
            logger.LogInformation(
                "Test {Endpoint} {Id}: HTTP {(int)resp.StatusCode}",
                endpoint,
                webhookId,
                (int)resp.StatusCode
            );
        return body;
    }
}
