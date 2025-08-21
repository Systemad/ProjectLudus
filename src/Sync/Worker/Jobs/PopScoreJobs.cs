
using Marten;
using Shared.Features.PopScore;
using TickerQ.Utilities.Base;


namespace Worker;


public class PopScoreJobs
{
    private ApiClient _apiClient;
    private readonly IDocumentStore _store;

    public PopScoreJobs(ApiClient apiClient, IDocumentStore store)
    {
        _apiClient = apiClient;
        _store = store;
    }

    [TickerFunction(functionName: "PopScoreFetch", cronExpression: "55 23 * * *")]
    public async Task StoreLatestPopScoreGamesAsync()
    {
        var types = new List<int>{1, 5, 6, 8};
        var popscoreGames = new List<PopScoreGame>();
        foreach (var type in types)
        {
            var items = await _apiClient.FetchPopScoreGames(new []{type});
            popscoreGames.AddRange(items);
        }

        await using var session = _store.LightweightSession();
        session.StoreObjects(popscoreGames);
        await session.SaveChangesAsync();
    }
}