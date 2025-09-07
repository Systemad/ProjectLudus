using Shared.Features.PopScore;
using TickerQ.Utilities.Base;

namespace Worker.Jobs;

public class PopScoreJobs
{
    private ApiClient _apiClient;

    public PopScoreJobs(ApiClient apiClient)
    {
        _apiClient = apiClient;
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

   
    }
}