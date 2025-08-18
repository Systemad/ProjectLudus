using Marten;
using Shared.Features;

namespace Worker;

public class JsonSeederService
{
    private readonly IDocumentStore _store;
    private const string JsonFilePath = "Cache/gamefile.json";

    public JsonSeederService(IDocumentStore store)
    {
        _store = store;
    }

    public async Task SeedAsync(bool reset = false)
    {
        
        if (reset)
        {
            await _store.Advanced.Clean.CompletelyRemoveAllAsync();
            await _store.Advanced.Clean.DeleteAllDocumentsAsync();
            await _store.Advanced.Clean.DeleteAllEventDataAsync();
        }
        
        await using var fs = File.OpenRead(JsonFilePath);

        var games = await OptimizedList.ReadFromStreamAsync(fs);

        var batchSize = 500;
        var batch = new List<IGDBGameRaw>(batchSize);

        foreach (var game in games)
        {
            batch.Add(game);

            if (batch.Count >= batchSize)
            {
                await InsertGamesBatchAsync(batch);
                batch.Clear();
            }
        }

        if (batch.Count > 0)
            await InsertGamesBatchAsync(batch);
    }

    private async Task InsertGamesBatchAsync(
        List<IGDBGameRaw> games
    )
    {
        var inserData = new InsertData();
        var flattened = games.NormalizeGames();
        await _store.BulkInsertAsync(flattened, BulkInsertMode.IgnoreDuplicates);

        inserData.GameModes.AddRange(Utilities.GetDistinctEntities(games, g => g.GameModes));
        inserData.Genres.AddRange(Utilities.GetDistinctEntities(games, g => g.Genres));
        inserData.Platforms.AddRange(Utilities.GetDistinctEntities(games, g => g.Platforms));
        inserData.PlayerPerspectives.AddRange(Utilities.GetDistinctEntities(games, g => g.PlayerPerspectives));
        inserData.GameEngines.AddRange(Utilities.GetDistinctEntities(games, g => g.GameEngines));
        inserData.Themes.AddRange(Utilities.GetDistinctEntities(games, g => g.Themes));
        inserData.Franchises.AddRange(Utilities.GetDistinctEntities(games, g => g.Franchises));
        inserData.Keywords.AddRange(Utilities.GetDistinctEntities(games, g => g.Keywords));
        await InsertDataAsync(inserData);
        
        inserData.GameModes.Clear();
        inserData.Genres.Clear();
        inserData.Platforms.Clear();
        inserData.PlayerPerspectives.Clear();
        inserData.GameEngines.Clear();
        inserData.Themes.Clear();
        inserData.Franchises.Clear();
        inserData.Keywords.Clear();
    }

    private async Task InsertDataAsync(InsertData insertData)
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
}