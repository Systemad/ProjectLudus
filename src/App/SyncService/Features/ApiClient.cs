using System.Text;
using Shared.Features;
using Shared.Features.Games;
using Shared.Features.PopScore;
using Shared.Queries;
using Shared.Twitch;

namespace SyncService.Features;

public class ApiClient(HttpClient httpClient)
{
    /*
     * create
     * delete
     * update
    */

    
    public async Task<CountResponse> FetchCountAsync(string url)
    {
        var response = await httpClient.PostAsync($"{url}/count", null);
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

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var batch = await response.Content.ReadFromJsonAsync<List<T>>();

        if (batch == null || batch.Count == 0)
            throw new ArgumentException("Items are null");

        return batch;
    }

    public async Task<IGDBGame> FetchGameAsync(long id)
    {
        var requestBody =
            $"fields {string.Join(",", GameQuery.Fields)}; where {id};";

        using var request = new HttpRequestMessage(HttpMethod.Post, GameQuery.Url);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var game = await response.Content.ReadFromJsonAsync<IGDBGame>();

        if (game == null)
            throw new ArgumentException("Games is null");

        return game;
    }
    
    public async Task<List<IGDBGame>> FetchBatchAsync(long itemsToTake, long offset)
    {
        var requestBody =
            $"fields {string.Join(",", GameQuery.Fields)}; limit {itemsToTake}; offset {offset};";

        using var request = new HttpRequestMessage(HttpMethod.Post, GameQuery.Url);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await httpClient.SendAsync(request);
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

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var gamesTypes = await response.Content.ReadFromJsonAsync<List<GameType>>();
        var mapped = gamesTypes.Select(x => new InternalGameType
        {
            OriginalId = x.Id,
            Type = x.Type,
        });
        return mapped.ToArray();
    }
    
    public async Task<List<PopularityTypes>> FetchPopScoreTypes()
    {
        var requestBody =
            $"fields {string.Join(",", PopScoreTypesQuery.Fields)}; sort id asc;";

        using var request = new HttpRequestMessage(HttpMethod.Post, PopScoreTypesQuery.Url);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var popscoreTypes = await response.Content.ReadFromJsonAsync<List<PopularityTypes>>();

        if (popscoreTypes == null || popscoreTypes.Count == 0)
            throw new ArgumentException("PopScoreTypes are null");

        return popscoreTypes;
    }
    
    public async Task<List<PopScoreGame>> FetchPopScoreGames(int[] types, int limit = 50)
    {
        // fields game_id,value,popularity_type; sort value desc; limit 50; where popularity_type = (5, 6, 8);
        var requestBody =
            $"fields {string.Join(",", PopScoreGamesQuery.Fields)}; sort value asc; limit {limit}; where popularity_type = ({string.Join(",", types)})";

        using var request = new HttpRequestMessage(HttpMethod.Post, PopScoreTypesQuery.Url);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var popscoreGames = await response.Content.ReadFromJsonAsync<List<PopScoreGame>>();

        if (popscoreGames == null || popscoreGames.Count == 0)
            throw new ArgumentException("popscoreGames are null");

        return popscoreGames;
    }
}
