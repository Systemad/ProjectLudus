using Ludus.Server.Features.User.Handlers;

namespace Ludus.Server.Features.User;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/user").WithTags("User").WithOpenApi();
        group.MapGet("me", MeAsync.Handler);

        return group;
    }
}
