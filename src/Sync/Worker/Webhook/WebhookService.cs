using Microsoft.Extensions.Options;
using Shared.Twitch;

namespace Worker.Webhook;

public class WebhookService
{
    private HttpClient _client;
    private readonly IHttpClientFactory _httpClientFactory;

    private TwitchOptions _twithOptions;

    public WebhookService(
        HttpClient client,
        IHttpClientFactory httpClientFactory,
        IOptions<TwitchOptions> options
    )
    {
        _client = client;
        _httpClientFactory = httpClientFactory;

        _twithOptions = options.Value;

        if (
            string.IsNullOrEmpty(_twithOptions.IGDB_CLIENT_ID)
            || string.IsNullOrEmpty(_twithOptions.IGDB_CLIENT_SECRET)
        )
        {
            throw new ArgumentException("ClientID and token must be supplied!");
        }
    }

    public async Task SubscribeAsync()
    {

    }

    public async Task UnsubscribeAllAsync()
    {
    }
}
