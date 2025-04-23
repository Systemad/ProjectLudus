using System.Text;
using Ludus.Service.Twitch;
using Ludus.Shared;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Schema;

namespace Ludus.Service;

public class DataSeeder
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDocumentStore _store;
    private readonly ITwitchAccessTokenService _accessTokenService;
    private HttpClient _client;

    public DataSeeder(
        IHttpClientFactory httpClientFactory,
        IDocumentStore store,
        ITwitchAccessTokenService accessTokenService,
        HttpClient client
    )
    {
        _httpClientFactory = httpClientFactory;
        _store = store;
        _accessTokenService = accessTokenService;
        _client = client;
    }

    // TODO: remove keys
    public async Task Populate()
    {
        _client = _httpClientFactory.CreateClient("IGDB");
        _client.DefaultRequestHeaders.Add(
            "Client-ID",
            "i0s32q3oi8z074rvq0ljkaupnbkq98" /*options.ClientId*/
        );
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer x8hmb9ft7j8mohy6yjfdm9ano95abm");

        var response = await _client.PostAsync("games/count", null);
        var countResponse = await response.Content.ReadFromJsonAsync<CountResponse>();

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        await using var session = _store.LightweightSession();

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            Console.WriteLine($"Fetching {itemsToTake} items!");
            await FetchAndInsertGamesAsync(itemsToTake, offset);
            await Task.Delay(300);
        }
        Console.WriteLine("Fetched all games!");
        await Task.CompletedTask;
    }

    private async Task FetchAndInsertGamesAsync(long itemsToTake, long offset)
    {
        await using var session = _store.LightweightSession();
        var requestBody =
            $"fields {string.Join(",", Query.Fields)}; limit {itemsToTake}; offset {offset};";

        using var request = new HttpRequestMessage(HttpMethod.Post, "games");
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var games = await response.Content.ReadFromJsonAsync<List<Game>>();

        if (games == null || games.Count == 0)
            return;

        var gameModes = GetDistinctEntities(games, g => g.GameModes);
        var genres = GetDistinctEntities(games, g => g.Genres);
        var involvedCompanies = GetDistinctEntities(games, g => g.InvolvedCompanies);
        var platforms = GetDistinctEntities(games, g => g.Platforms);
        var playerPerspectives = GetDistinctEntities(games, g => g.PlayerPerspectives);
        var gameEngines = GetDistinctEntities(games, g => g.GameEngines);
        var themes = GetDistinctEntities(games, g => g.Themes);
        var franchises = GetDistinctEntities(games, g => g.Franchises);

        var gameTypes = games
            .Select(g => g.GameType)
            .Where(gt => gt != null && gt.Id > 0)
            .GroupBy(gt => gt.Id)
            .Select(g => g.First())
            .ToList();

        session.StoreObjects(gameModes);
        session.StoreObjects(genres);
        session.StoreObjects(involvedCompanies);
        session.StoreObjects(platforms);
        session.StoreObjects(playerPerspectives);
        session.StoreObjects(gameEngines);
        session.StoreObjects(themes);
        session.StoreObjects(franchises);
        session.StoreObjects(gameTypes);

        await session.DocumentStore.BulkInsertAsync(games, BulkInsertMode.OverwriteExisting);
        await session.SaveChangesAsync();
        //await session.DocumentStore.BulkInsertAsync(games, BulkInsertMode.InsertsOnly);
    }

    private async Task FetchGamesTypesAsync()
    {
        await using var session = _store.LightweightSession();
        var requestBody = $"fields {string.Join(",", Query.Fields)};;";

        using var request = new HttpRequestMessage(HttpMethod.Post, "games");
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var games = await response.Content.ReadFromJsonAsync<List<Game>>();
    }

    private static IList<T> GetDistinctEntities<T>(
        IEnumerable<Game> games,
        Func<Game, IEnumerable<T>> selector
    )
        where T : class
    {
        return games
            .SelectMany(g => selector(g) ?? Enumerable.Empty<T>())
            //.SelectMany(selector)
            .DistinctBy<T, long>(e => (e as dynamic).Id) // assuming Id is int
            .ToList();
    }
}

/*
 *         var gameModes = games.SelectMany(g => g.GameModes).DistinctBy(gm => gm.Id).ToList();
        var genres = games.SelectMany(g => g.Genres).DistinctBy(gm => gm.Id).ToList();
        var involvedCompanies = games
            .SelectMany(g => g.InvolvedCompanies)
            .DistinctBy(gm => gm.Id)
            .ToList();
        var platforms = games.SelectMany(g => g.Platforms).DistinctBy(gm => gm.Id).ToList();
        var playerPerspectives = games
            .SelectMany(g => g.PlayerPerspectives)
            .DistinctBy(gm => gm.Id)
            .ToList();
        var gameEngines = games.SelectMany(g => g.GameEngines).DistinctBy(gm => gm.Id).ToList();
        var franchise = games.SelectMany(g => g.Franchises).DistinctBy(gm => gm.Id).ToList();
 */
