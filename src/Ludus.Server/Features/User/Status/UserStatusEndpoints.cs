using Ludus.Server.Features.User.Status.Handlers;

namespace Ludus.Server.Features.User.Status;

public static class UserStatusEndpoints
{
    public static RouteGroupBuilder MapUserStatusEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/user/status").WithTags("Status").WithOpenApi();
        group.MapGet("/", GetMyGameStatusesAsync.Handler).RequireAuthorization();
        group.MapPut("/{gameId:long}", UpdateGameStatusAsync.Handler).RequireAuthorization();
        group.MapGet("/{userId:guid}", GetGameStatusesAsync.Handler).RequireAuthorization("Admin");

        return group;
    }
}
