using System.Text.Json;
using Marten;
using Shared.Features.Games;
using Shared.Queries;

namespace IGDBService;

public class SeederService
{
    private readonly IDocumentStore _store;
    private ApiClient _apiClient;

    private const string gameFile = "Cache/gamefile.json";
    private const string companyFile = "Cache/companies.json";

    public SeederService(IDocumentStore store, ApiClient apiClient)
    {
        _store = store;
        _apiClient = apiClient;
    }

    public async Task PopulateGamesAsync(bool writeToCache = false, bool reset = false)
    {
        if (reset)
        {
            await _store.Advanced.Clean.CompletelyRemoveAllAsync();
            await _store.Advanced.Clean.DeleteAllDocumentsAsync();
            await _store.Advanced.Clean.DeleteAllEventDataAsync();
        }

        var countResponse = await _apiClient.FetchGamesCountAsync();

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        var allGames = new List<IGDBGame>();

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            var games = await InsertGamesBatchAsync(itemsToTake, offset);
            allGames.AddRange(games);
            Console.WriteLine(
                $"Batch {i + 1}/{iterations} — Fetched and stored {games.Count} games."
            );
            await Task.Delay(100);
        }

        //var gameTypes = await _apiClient.FetchGamesTypesAsync();
        //session.StoreObjects(gameTypes);
        if (writeToCache)
        {
            await WriteToJsonCacheAsync(allGames);
        }

        await Task.CompletedTask;
    }

    private async Task<List<IGDBGame>> InsertGamesBatchAsync(
        long itemsToTake,
        long offset
    )
    {
        var inserData = new InsertData();
        var games = await _apiClient.FetchBatchAsync(itemsToTake, offset);
        await _store.BulkInsertAsync(games, BulkInsertMode.OverwriteExisting);


        // item.GameModes
        inserData.GameModes.AddRange(GetDistinctEntities(games, g => g.GameModes));

        //inserData.Genres.AddRange(item.Genres);
        inserData.Genres.AddRange(GetDistinctEntities(games, g => g.Genres));

        //inserData.Platforms.AddRange(item.Platforms);
        inserData.Platforms.AddRange(GetDistinctEntities(games, g => g.Platforms));

        //inserData.PlayerPerspectives.AddRange(item.PlayerPerspectives);
        inserData.PlayerPerspectives.AddRange(GetDistinctEntities(games, g => g.PlayerPerspectives));

        //inserData.GameEngines.AddRange(item.GameEngines);
        inserData.GameEngines.AddRange(GetDistinctEntities(games, g => g.GameEngines));

        //inserData.Themes.AddRange(item.Themes);
        inserData.Themes.AddRange(GetDistinctEntities(games, g => g.Themes));

        //inserData.Franchises.AddRange(item.Franchises);
        inserData.Franchises.AddRange(GetDistinctEntities(games, g => g.Franchises));

        //inserData.Keywords.AddRange(item.Keywords);
        inserData.Keywords.AddRange(GetDistinctEntities(games, g => g.Keywords));
        await InserDataAsync(inserData);
        return games;
    }

    private async Task InserDataAsync(InsertData insertData)
    {
        await _store.BulkInsertAsync(insertData.GameModes, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Genres, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Platforms, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(
            insertData.PlayerPerspectives,
            BulkInsertMode.OverwriteExisting
        );
        await _store.BulkInsertAsync(insertData.GameEngines, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Themes, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Franchises, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Keywords, BulkInsertMode.OverwriteExisting);
    }

    private async Task WriteToJsonCacheAsync(List<IGDBGame> games)
    {
        await using var stream = new StreamWriter(gameFile, append: true);
        var options = new JsonSerializerOptions { WriteIndented = true };
        foreach (var game in games)
        {
            string json = JsonSerializer.Serialize(game, options);
            await stream.WriteLineAsync(json);
        }
    }

    public async Task PopulateCompaniesAsync()
    {
        var countResponse = await _apiClient.FetchGamesCountGeneric(CompanyQuery.Count);

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            Console.WriteLine($"Fetching {itemsToTake} items!");
            await InsertCompanyBatchAsync(itemsToTake, offset);
            await Task.Delay(300);
        }

        await Task.CompletedTask;
    }

    private async Task InsertCompanyBatchAsync(long itemsToTake, long offset)
    {
        await using var session = _store.LightweightSession();
        var companies = await _apiClient.FetchBatchAsyncGeneric<RawCompany>(
            CompanyQuery.Url,
            CompanyQuery.Fields,
            itemsToTake,
            offset
        );

        await session.DocumentStore.BulkInsertAsync(companies, BulkInsertMode.OverwriteExisting);
        await session.SaveChangesAsync();
    }

    private static List<T> GetDistinctEntities<T>(
        IEnumerable<IGDBGame> games,
        Func<IGDBGame, IEnumerable<T>> selector
    )
        where T : class
    {
        return games
            .SelectMany(g => selector(g) ?? Enumerable.Empty<T>())
            //.SelectMany(selector)
            .DistinctBy<T, long>(e => (e as dynamic).Id) // assuming Id is int
            .ToList();
    }

    private static List<T> GetDistinctEntities2<T>(IEnumerable<T> items) where T : class
    {
        return items
            .DistinctBy(e => (e as dynamic).Id)
            .ToList();
    }
}

/*
var gameModes = GetDistinctEntities(games, g => g.GameModes);
var genres = GetDistinctEntities(games, g => g.Genres);
var involvedCompanies = GetDistinctEntities(games, g => g.InvolvedCompanies);
var platforms = GetDistinctEntities(games, g => g.Platforms);
var playerPerspectives = GetDistinctEntities(games, g => g.PlayerPerspectives);
var gameEngines = GetDistinctEntities(games, g => g.GameEngines);
var themes = GetDistinctEntities(games, g => g.Themes);
var franchises = GetDistinctEntities(games, g => g.Franchises);

session.StoreObjects(gameModes);
session.StoreObjects(genres);
session.StoreObjects(involvedCompanies);
session.StoreObjects(platforms);
session.StoreObjects(playerPerspectives);
session.StoreObjects(gameEngines);
session.StoreObjects(themes);
session.StoreObjects(franchises);
*/