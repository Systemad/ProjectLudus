#:property LangVersion=preview
#:project ./src/Shared/Shared.csproj
#:project ./src/Sync/Worker/Worker.csproj
#:package Marten@8.7.0

#:property UserSecretsId=696f31a6-b46a-47e0-b35a-385e9e205a05
using Marten;
using Shared;

string url = "host=localhost:5432;database=ludustest;CommandTimeout=500;password=Compaq2009;username=postgres";

var store = DocumentStore
    .For(options =>
    {
        options.Connection(url);
        options.UseSystemTextJsonForSerialization();
        MartenSchema.Configure(options);
    });
    //.UseNpgsqlDataSource();

Console.WriteLine("From [CallerFilePath] attribute:");
Console.WriteLine($" - Entry-point path: {Path.EntryPointFilePath()}");
Console.WriteLine($" - Entry-point directory: {Path.EntryPointFileDirectoryPath()}");

Console.WriteLine("From AppContext data:");
Console.WriteLine($" - Entry-point path: {AppContext.EntryPointFilePath()}");
Console.WriteLine($" - Entry-point directory: {AppContext.EntryPointFileDirectoryPath()}");

var apiClient = new ApiClient("YOUR_IGDB_KEY");
var seeder = new GameSeeder(store, apiClient);

var config = new ConfigurationBuilder()
    .AddUserSecrets()  // This will use the ID above
    .Build();



static class PathEntryPointExtensions
{
    extension(Path)
    {
        public static string EntryPointFilePath() => EntryPointImpl();

        public static string EntryPointFileDirectoryPath() => Path.GetDirectoryName(EntryPointImpl()) ?? "";

        private static string EntryPointImpl([System.Runtime.CompilerServices.CallerFilePath] string filePath = "") => filePath;
    }
}

static class AppContextExtensions
{
    extension(AppContext)
    {
        public static string? EntryPointFilePath() => AppContext.GetData("EntryPointFilePath") as string;
        public static string? EntryPointFileDirectoryPath() => AppContext.GetData("EntryPointFileDirectoryPath") as string;
    }
}