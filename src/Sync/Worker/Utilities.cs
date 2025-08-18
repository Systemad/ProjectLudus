using Shared.Features;

namespace Worker;

public static class Utilities
{
    public static string? GetConnectionString(string name = "ludustest")
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(path))
        {
            Console.WriteLine($"⚠️ appsettings.json not found at: {path}");
            return null;
        }

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        return config.GetConnectionString(name);
    }
    
    public static List<T> GetDistinctEntities<T>(
        IEnumerable<IGDBGameRaw> games,
        Func<IGDBGameRaw, IEnumerable<T>> selector
    )
        where T : class
    {
        return games
            .SelectMany(g => selector(g) ?? Enumerable.Empty<T>())
            //.SelectMany(selector)
            .DistinctBy<T, long>(e => (e as dynamic).Id) // assuming Id is int
            .ToList();
    }
}