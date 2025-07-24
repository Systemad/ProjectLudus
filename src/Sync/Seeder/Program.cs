// See https://aka.ms/new-console-template for more information

using D20Tek.Spectre.Console.Extensions;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seeder;
using Shared.Twitch;
using Spectre.Console.Cli;
using Weasel.Core;

var serviceCollection = new ServiceCollection();

var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

var configurationRoot = builder.Build();

serviceCollection.Configure<TwitchOptions>(configurationRoot.GetSection("Twitch"));
serviceCollection.AddHttpClient(
    "IGDB",
    httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
);
serviceCollection.AddScoped<ApiClient>();
serviceCollection.AddScoped<SeederService>();

serviceCollection.AddMarten(options =>
{
    options.Connection("host=localhost:5432;database=gamingdb;password=Compaq2009;username=dan1");
    options.UseSystemTextJsonForSerialization();
    options.AutoCreateSchemaObjects = AutoCreate.All;
});

var app = new CommandApp<SeedCommand>();
app.Configure(config =>
{
    config.CaseSensitivity(CaseSensitivity.All);
    config.SetApplicationName("Seeder");
    config.ValidateExamples();
    
    config.AddCommand<SeedCommand>("default")
        .WithDescription("Default command that displays some text.")
        .WithExample(new[] { "default", "--verbose", "high" });
});

await app.RunAsync(args);

