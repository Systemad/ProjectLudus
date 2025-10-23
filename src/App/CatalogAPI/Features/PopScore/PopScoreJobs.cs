using CatalogAPI.Data;
using IGDB;
using IGDB.Models;
using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using TickerQ.Utilities.Base;

namespace CatalogAPI.Features.PopScore;

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

        await _dbContext.PopScoreGames.ExecuteBulkInsertAsync(
            popscoreGames,
            options =>
            {
                options.BatchSize = 500;
            },
            onConflict: new OnConflictOptions<PopularityPrimitive>
            {
                Match = e => new { e.GameId },

                Update = (inserted, excluded) => inserted,
            }
        );
        */
    }
}
