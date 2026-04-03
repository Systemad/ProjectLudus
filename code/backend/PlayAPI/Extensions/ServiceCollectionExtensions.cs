using PlayAPI.Client.TypesenseClient;
using PlayAPI.Features.GameClicks;
using PlayAPI.Features.Typesense;
using Refit;

namespace PlayAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameClickTrackingServices(this IServiceCollection services)
    {
        services.AddScoped<GameClickTrackingService>();

        return services;
    }

    public static IServiceCollection AddTypesenseServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<TypesenseKeyService>();
        services.AddScoped<ICreateKeyEndpoint>(_ =>
        {
            var adminKey = configuration["TYPESENSE_API_KEY"];

            if (string.IsNullOrWhiteSpace(adminKey))
            {
                throw new InvalidOperationException(
                    "Missing required configuration key: TYPESENSE_API_KEY"
                );
            }

            var baseUrl = configuration["TYPESENSE-URL"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = "http://localhost:8108";
            }

            var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            httpClient.DefaultRequestHeaders.Add("X-TYPESENSE-API-KEY", adminKey);

            return RestService.For<ICreateKeyEndpoint>(httpClient);
        });

        return services;
    }
}
