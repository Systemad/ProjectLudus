using System.Net.Http.Json;
using System.Text;
using Ludus.Data;
using Ludus.Shared;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Seeder;

public class GameSeeder
{
    /*
    private HttpClient _httpClient;
    private readonly IDbContextFactory<GamesDbContext> _dbContextFactory;

    public GameSeeder(HttpClient httpClient, IDbContextFactory<GamesDbContext> dbContextFactory)
    {
        _httpClient = httpClient;
        _dbContextFactory = dbContextFactory;
        _httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
    }

    public async Task SeedDatabaseAsync()
    {
        var countResponse = await _httpClient.GetFromJsonAsync<CountResponse>("games/count");

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration; // Correct offset calculation
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            await FetchAndStoreGamesAsync(itemsToTake, offset); // Fetch and insert into DB
            await Task.Delay(500); // Respect API rate limits
        }
    }

    private async Task FetchAndStoreGamesAsync(long itemsToTake, long offset)
    {
        var requestBody = $"fields {Query.Fields}; limit {itemsToTake}; offset {offset};";
        using var request = new HttpRequestMessage(HttpMethod.Post, "games")
        {
            Content = new StringContent(requestBody, Encoding.UTF8, "text/plain"),
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var games = await request.Content.ReadFromJsonAsync<List<Game>>();

        if (games == null || games.Count == 0)
            return;
        using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Games.AddRange(games);
        dbContext.SaveChanges();
    }
    */
}
