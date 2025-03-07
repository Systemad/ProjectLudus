using Shared;

namespace Seed;

public class Tasks
{
    private readonly Fetch _fetch;
    
    public async Task FetchAndInsertDataAsync(HttpClient client, CancellationToken cancellationToken)
    {
        using var db = new SeedContext();
        var tasks = new List<Task>();

        foreach (var entry in Query.DataConfig)
        {
            var endpoint = entry.Key;
            var fields = entry.Value;

            Task fetchtask = endpoint switch
            {
                "age_rating_organizations" => _fetch.FetchAllDataAsync<AgeRatingOrganizations>(endpoint, fields, cancellationToken),
                _ => throw new ArgumentOutOfRangeException()
            };
            tasks.Add(fetchtask);
        }

        // Execute all tasks concurrently
        await Task.WhenAll(tasks);
    }
}