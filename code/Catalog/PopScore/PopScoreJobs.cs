using Catalog.Data;
using IGDB;

namespace Catalog.PopScore;

// TODO: RETHINK THIS APPROAH
public class PopScoreJobs
{
    private IGDBClient _client;
    private CatalogContext _dbContext;

    public PopScoreJobs(CatalogContext dbContext, IGDBClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }

    [TickerFunction(functionName: "PopScoreFetch", cronExpression: "55 23 * * *")]
    public async Task StoreLatestPopScoreGamesAsync()
    {
        await Task.CompletedTask;
        /*
        var types = new List<int> { 1, 5, 6, 8 };
        var popscoreGames = new List<PopularityPrimitive>();
        foreach (var type in types)
        {
            var items = await _client.QueryAsync<PopularityPrimitive>(IGDBClient.Endpoints.PopularityPrimitives,
                query: "id,popularity_source,name,updated_at,external_popularity_source.name");
            popscoreGames.AddRange(items);
        }

            // sent message to qeue, "popscoreUpdatedEvent
        */
    }
}
