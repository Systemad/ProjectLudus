using System.Text;
using IGDBService.Twitch;
using Microsoft.Extensions.Options;
using Shared;
using Shared.Features.Games;

namespace IGDBService;

public class ApiClient
{
    private HttpClient _client;
    private readonly IHttpClientFactory _httpClientFactory;

    private TwitchOptions _twithOptions;

    public ApiClient(
        HttpClient client,
        IHttpClientFactory httpClientFactory,
        IOptions<TwitchOptions> options
    )
    {
        _client = client;
        _httpClientFactory = httpClientFactory;

        _twithOptions = options.Value;

        if (_twithOptions.ClientId == string.Empty || _twithOptions.ClientSecret == string.Empty)
        {
            throw new ArgumentException("ClientID and token must be supplied!");
        }
    }

    public async Task<CountResponse> FetchGamesCountAsync()
    {
        _client = _httpClientFactory.CreateClient("IGDB");
        _client.DefaultRequestHeaders.Add("Client-ID", $"{_twithOptions.ClientId}");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_twithOptions.ClientSecret} ");

        var response = await _client.PostAsync("games/count", null);
        var countResponse = await response.Content.ReadFromJsonAsync<CountResponse>();
        if (countResponse is null)
            throw new ArgumentException("Count response is null!");

        return countResponse;
    }

    public async Task<List<RawGame>> FetchBatchAsync(long itemsToTake, long offset)
    {
        var requestBody =
            $"fields {string.Join(",", Query.Fields)}; limit {itemsToTake}; offset {offset};";

        using var request = new HttpRequestMessage(HttpMethod.Post, "games");
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var games = await response.Content.ReadFromJsonAsync<List<RawGame>>();

        if (games == null || games.Count == 0)
            throw new ArgumentException("Games is null");

        return games;
    }

    public async Task<InternalGameType[]> FetchGamesTypesAsync()
    {
        var requestBody = $"fields type; limit 500;";

        using var request = new HttpRequestMessage(HttpMethod.Post, "game_types");
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var gamesTypes = await response.Content.ReadFromJsonAsync<List<GameType>>();
        var mapped = gamesTypes.Select(x => new InternalGameType
        {
            OriginalId = x.Id,
            Type = x.Type,
        });
        return mapped.ToArray();
    }
}
