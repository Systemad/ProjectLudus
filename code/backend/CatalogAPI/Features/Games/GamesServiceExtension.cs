using CatalogAPI.Features.Games.GetByReleaseDateRange;
using FluentValidation;

namespace CatalogAPI.Features.Games;

public static class GamesServiceExtension
{
    public static IServiceCollection AddGamesServices(this IServiceCollection services)
    {
        services.RegisterGamesValidations();
        return services;
    }
    
    public static IServiceCollection RegisterGamesValidations(this IServiceCollection services)
    {
        services.AddScoped<IValidator<GetByReleaseDateRangeQuery>, GetByReleaseDateValidator>();
        return services;
    }
}