using System.Text;
using Ludus.Service.Twitch;
using Ludus.Shared;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Service;

public class DataSeeder(
    IHttpClientFactory httpClientFactory,
    IDocumentSession session,
    ITwitchAccessTokenService accessTokenService
) : IDataSeeder, IDisposable
{
    private HttpClient _client;

    public async Task Seed()
    {
        _client = httpClientFactory.CreateClient("IGDB");
        _client.DefaultRequestHeaders.Add(
            "Client-ID",
            "" /*options.ClientId*/
        );
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer ");

        var response = await _client.PostAsync("games/count", null);
        var countResponse = await response.Content.ReadFromJsonAsync<CountResponse>();

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            Console.WriteLine($"Fetching {itemsToTake} items!");
            await FetchAndStoreGamesAsync(itemsToTake, offset);
            await Task.Delay(300);
        }

        Console.WriteLine("Fetched all games!");
    }

    public async Task FetchAndStoreGamesAsync(long itemsToTake, long offset)
    {
        var requestBody =
            $"fields {string.Join(",", Query.Fields)}; limit {itemsToTake}; offset {offset};";

        using var request = new HttpRequestMessage(HttpMethod.Post, "games");
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var games = await response.Content.ReadFromJsonAsync<List<Game>>();

        if (games == null || games.Count == 0)
            return;

        await session.DocumentStore.BulkInsertAsync(games, BulkInsertMode.OverwriteExisting);
    }

    public void Dispose() => _client.Dispose();
}
