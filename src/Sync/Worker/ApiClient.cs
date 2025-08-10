using System.Text;
using Microsoft.Extensions.Options;
using Shared.Features;
using Shared.Features.Games;
using Shared.Queries;
using Shared.Twitch;

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

        if (
            string.IsNullOrEmpty(_twithOptions.ClientId)
            || string.IsNullOrEmpty(_twithOptions.ClientSecret)
        )
        {
            throw new ArgumentException("ClientID and token must be supplied!");
        }
    }

    public async Task<CountResponse> FetchGamesCountAsync()
    {
        _client = _httpClientFactory.CreateClient("IGDB");
        _client.DefaultRequestHeaders.Add("Client-ID", $"{_twithOptions.ClientId}");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_twithOptions.ClientSecret}");

        var response = await _client.PostAsync(GameQuery.Count, null);
        var countResponse = await response.Content.ReadFromJsonAsync<CountResponse>();
        if (countResponse is null)
            throw new ArgumentException("Count response is null!");

        return countResponse;
    }

    public async Task<CountResponse> FetchCountAsync(string url)
    {
        _client = _httpClientFactory.CreateClient("IGDB");
        _client.DefaultRequestHeaders.Add("Client-ID", $"{_twithOptions.ClientId}");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_twithOptions.ClientSecret}");

        var response = await _client.PostAsync($"{url}/count", null);
        var countResponse = await response.Content.ReadFromJsonAsync<CountResponse>();
        if (countResponse is null)
            throw new ArgumentException("Count response is null!");

        return countResponse;
    }

    public async Task<List<T>> FetchBatchAsyncGeneric<T>(
        string url,
        List<string> fields,
        long itemsToTake,
        long offset
    )
    {
        var requestBody =
            $"fields {string.Join(",", fields)}; limit {itemsToTake}; offset {offset};";

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var batch = await response.Content.ReadFromJsonAsync<List<T>>();

        if (batch == null || batch.Count == 0)
            throw new ArgumentException("Items are null");

        return batch;
    }

    public async Task<List<IGDBGame>> FetchBatchAsync(long itemsToTake, long offset)
    {
        var requestBody =
            $"fields {string.Join(",", GameQuery.Fields)}; limit {itemsToTake}; offset {offset};";

        using var request = new HttpRequestMessage(HttpMethod.Post, GameQuery.Url);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var games = await response.Content.ReadFromJsonAsync<List<IGDBGame>>();

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
