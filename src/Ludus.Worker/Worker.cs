using System.Net.Http.Json;
using System.Text;
using Ludus.Shared;
using Marten;
using Marten.Internal.Storage;

namespace Ludus.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    //private IDocumentStorage _document;
    private IDocumentSession _session;
    private readonly IServiceProvider _container;
    private IServiceScopeFactory _serviceScopeFactory;

    public Worker(
        ILogger<Worker> logger,
        IServiceProvider container,
        IServiceScopeFactory serviceScopeFactory
    //IDocumentStorage document,
    //IDocumentSession session
    )
    {
        _logger = logger;
        _container = container;
        _serviceScopeFactory = serviceScopeFactory;
        //_document = document;
        //_session = session;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        _session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        httpClient.DefaultRequestHeaders.Add("Client-ID", "");
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer ");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        var response = await httpClient.PostAsync("games/count", null);
        var countResponse = await response.Content.ReadFromJsonAsync<CountResponse>();

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration; // Correct offset calculation
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            Console.WriteLine($"Fetching {itemsToTake} items!");
            await FetchAndStoreGamesAsync(httpClient, itemsToTake, offset); // Fetch and insert into DB
            await Task.Delay(300); // Respect API rate limits
        }

        Console.WriteLine("Fetched all games!");
    }

    private async Task FetchAndStoreGamesAsync(HttpClient httpClient, long itemsToTake, long offset)
    {
        var requestBody =
            $"fields {string.Join(",", Query.Fields)}; limit {itemsToTake}; offset {offset};";

        using var request = new HttpRequestMessage(HttpMethod.Post, "games");
        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var games = await response.Content.ReadFromJsonAsync<List<Game>>();

        if (games == null || games.Count == 0)
            return;

        await _session.DocumentStore.BulkInsertAsync(games, BulkInsertMode.OverwriteExisting);
    }
}
