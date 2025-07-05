using Microsoft.Extensions.Options;

namespace IGDBService.Twitch;

public class TwitchAccessTokenService : ITwitchAccessTokenService
{
    private IHttpClientFactory _httpClientFactory;
    private readonly TwitchOptions _options;
    private string _accessToken;

    public TwitchAccessTokenService(
        IHttpClientFactory httpClientFactory,
        IOptions<TwitchOptions> options
    )
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<string> FetchAccessTokenAsync()
    {
        if (
            string.IsNullOrWhiteSpace(_options.ClientId)
            || string.IsNullOrWhiteSpace(_options.ClientSecret)
        )
            throw new Exception("Twitch ClientId or ClientSecret is missing!");

        if (!string.IsNullOrWhiteSpace(_accessToken))
        {
            return _accessToken;
        }
        using var client = _httpClientFactory.CreateClient();
        var url =
            $"https://id.twitch.tv/oauth2/token?client_id={_options.ClientSecret}&client_secret={_options.ClientSecret}&grant_type=client_credentials";

        using var response = await client.PostAsync(url, null);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch access token. Status: {response.StatusCode}");
        }

        var token = await response.Content.ReadFromJsonAsync<TwitchBaseTokenResponse>();

        if (token == null || string.IsNullOrWhiteSpace(token.AccessToken))
            throw new Exception("Failed to fetch access token");
        _accessToken = token.AccessToken;
        return _accessToken;
    }

    public TwitchOptions GetCredentials() => _options;
}
