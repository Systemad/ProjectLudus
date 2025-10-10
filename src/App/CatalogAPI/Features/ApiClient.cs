using System.Text;
using CatalogAPI.Features.Games;
using CatalogAPI.Features.PopScore;
using Shared.Features;
using Shared.Features.Games;
using Shared.Features.IGDB;
using Shared.Features.PopScore;
using Shared.Twitch;

namespace CatalogAPI.Features;

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

        var items = await response.Content.ReadFromJsonAsync<List<T>>();

        if (items == null || items.Count == 0)
            throw new ArgumentException("Items are null");

        return items;
    }

    public async Task<List<T>> Fetch<T>(string endpoint, string url, List<string> fields)
    {
        var countResponse = await FetchCountAsync(url);

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        var items = new List<T>();
        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            var batch = await FetchBatchAsyncGeneric<T>(url, fields, itemsToTake, offset);
            items.AddRange(batch);
            await Task.Delay(200);
        }

        return items;
    }

    public async Task<IgdbGame> FetchGameAsync(long id)
    {
        var requestBody = $"fields {string.Join(",", GameQuery.Fields)}; where {id};";

        using var request = new HttpRequestMessage(HttpMethod.Post, GameQuery.Endpoint);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var game = await response.Content.ReadFromJsonAsync<IgdbGame>();

        if (game == null)
            throw new ArgumentException("Games is null");

        return game;
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
        var requestBody = $"fields {string.Join(",", PopScoreTypesQuery.Fields)}; sort id asc;";

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
