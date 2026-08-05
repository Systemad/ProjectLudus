using Catalog.Features.Games.Browse.GetByReleaseDateRange;

namespace Catalog.Features.Games;

public static class GamesServiceExtension
{
    public static IServiceCollection AddGamesServices(this IServiceCollection services)
    {
        services.RegisterGamesValidations();
        services.AddScoped<IGameService, GameService>();
        return services;
    }

    public static IServiceCollection RegisterGamesValidations(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Request>, GetByReleaseDateValidator>();
        return services;
    }
}
