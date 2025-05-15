#nullable enable
using Microsoft.Extensions.Http.Resilience;
using Refit;

namespace Ludus.Client
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureRefitClients(
            this IServiceCollection services,
            string baseAdress,
            Action<IHttpClientBuilder>? builder = default,
            RefitSettings? settings = default
        )
        {
            var clientBuilderIAuthEndpointsApi = services
                .AddRefitClient<IAuthEndpointsApi>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseAdress));

            clientBuilderIAuthEndpointsApi.AddStandardResilienceHandler(config =>
            {
                config.Retry = new HttpRetryStrategyOptions
                {
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(0.5),
                };
            });

            builder?.Invoke(clientBuilderIAuthEndpointsApi);

            var clientBuilderICollectionApi = services
                .AddRefitClient<ICollectionApi>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseAdress));

            clientBuilderICollectionApi.AddStandardResilienceHandler(config =>
            {
                config.Retry = new HttpRetryStrategyOptions
                {
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(0.5),
                };
            });

            builder?.Invoke(clientBuilderICollectionApi);

            var clientBuilderIListApi = services
                .AddRefitClient<IListApi>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseAdress));

            clientBuilderIListApi.AddStandardResilienceHandler(config =>
            {
                config.Retry = new HttpRetryStrategyOptions
                {
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(0.5),
                };
            });

            builder?.Invoke(clientBuilderIListApi);

            var clientBuilderIUserApi = services
                .AddRefitClient<IUserApi>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseAdress));

            clientBuilderIUserApi.AddStandardResilienceHandler(config =>
            {
                config.Retry = new HttpRetryStrategyOptions
                {
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(0.5),
                };
            });

            builder?.Invoke(clientBuilderIUserApi);

            var clientBuilderIGamesApi = services
                .AddRefitClient<IGamesApi>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseAdress));

            clientBuilderIGamesApi.AddStandardResilienceHandler(config =>
            {
                config.Retry = new HttpRetryStrategyOptions
                {
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(0.5),
                };
            });

            builder?.Invoke(clientBuilderIGamesApi);

            var clientBuilderIWeatherForecastApi = services
                .AddRefitClient<IWeatherForecastApi>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseAdress));

            clientBuilderIWeatherForecastApi.AddStandardResilienceHandler(config =>
            {
                config.Retry = new HttpRetryStrategyOptions
                {
                    UseJitter = true,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(0.5),
                };
            });

            builder?.Invoke(clientBuilderIWeatherForecastApi);

            return services;
        }
    }
}
