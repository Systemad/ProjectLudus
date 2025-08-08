namespace IGDBService;

public static class Utils
{
    public static string? GetConnectionString(string name = "Postgres")
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
}