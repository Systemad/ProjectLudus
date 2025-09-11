using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using Shared.Features.PopScore;
using SyncService.Data;
using TickerQ.Utilities.Base;

namespace SyncService.Jobs;

public class PopScoreJobs
{
    private ApiClient _apiClient;
    private AppDbContext _dbContext;

    public PopScoreJobs(ApiClient apiClient, AppDbContext dbContext)
    {
        _apiClient = apiClient;
        _dbContext = dbContext;
    }

    [TickerFunction(functionName: "PopScoreFetch", cronExpression: "55 23 * * *")]
    public async Task StoreLatestPopScoreGamesAsync()
    {
        var types = new List<int> { 1, 5, 6, 8 };
        var popscoreGames = new List<PopScoreGame>();
        foreach (var type in types)
        {
            var items = await _apiClient.FetchPopScoreGames(new[] { type });
            popscoreGames.AddRange(items);
        }

        await _dbContext.PopScoreGames.ExecuteBulkInsertAsync(popscoreGames, options => { options.BatchSize = 250; },
            onConflict: new OnConflictOptions<PopScoreGame>
            {
                Match = e => new
                {
                    e.Id,
                },

                Update = (inserted, excluded) => inserted
            });
    }
}