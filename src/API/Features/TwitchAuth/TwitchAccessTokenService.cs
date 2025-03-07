using Microsoft.Extensions.Caching.Memory;

namespace API.Features.TwitchAuth;

public class TwitchAccessTokenService : ITwitchAccessTokenService
{
    private IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private TwitchOptions _twitchOptions;
    
    private string _accessToken = "";
    //private string _refreshToken = "";

    public TwitchAccessTokenService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        
        _twitchOptions = _config.GetRequiredSection("Twitch").Get<TwitchOptions>();
    }

    public async Task<TwitchBaseTokenResponse> FetchAndSetAccessTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(_twitchOptions.ClientId) || string.IsNullOrWhiteSpace(_twitchOptions.ClientId))
            throw new Exception("Twitch ClientId or ClientSecret is missing!");
        
        using var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://id.twitch.tv/oauth2/token");
        var url =
            $"https://id.twitch.tv/oauth2/token?client_id={_twitchOptions.ClientId}&client_secret={_twitchOptions.ClientSecret}&grant_type=client_credentials";

        using var response = await client.PostAsync(url, null);
        var token = await response.Content.ReadFromJsonAsync<TwitchBaseTokenResponse>();

        if (token == null || string.IsNullOrWhiteSpace(token.AccessToken))
            throw new Exception("Failed to fetch access token");
        
        _accessToken = token.AccessToken;
        
        return token;
    }
    
    /*
    private async Task<TwitchBaseTokenResponse> FetchAndStoreAccessTokenAsync()
    {
        using var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://id.twitch.tv/oauth2/token");
        var url =
            $"https://id.twitch.tv/oauth2/token?client_id={_twitchOptions.ClientId}&client_secret={_twitchOptions.ClientSecret}&grant_type=client_credentials";

        using var response = await client.PostAsync(url, null);
        var fetchedToken = await response.Content.ReadFromJsonAsync<TwitchBaseTokenResponse>();

        if (fetchedToken == null || string.IsNullOrWhiteSpace(fetchedToken.AccessToken))
            throw new Exception("Failed to refresh access token");

        return fetchedToken;
    }
    */
    public string GetAccessToken() => _accessToken;
}