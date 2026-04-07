using PlayAPI.Client.TypesenseClient;
using PlayAPI.Features.Typesense.GetKey;
using Refit;

namespace PlayAPI.Features.Typesense;

public static class TypesenseServiceExtensions
{
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
