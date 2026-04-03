using PlayAPI.Features.GameClicks;
using PlayAPI.Features.Typesense;

namespace PlayAPI.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.UseTypesenseEndpoints();
        routeBuilder.UseGameClickEndpoints();

        return routeBuilder;
    }
}
