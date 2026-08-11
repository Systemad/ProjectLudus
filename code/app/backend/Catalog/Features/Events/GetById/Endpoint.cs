namespace Catalog.Features.Events.GetById;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        string id,
        IEventService eventService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(id, out var parsedId))
            return Results.BadRequest();

        var evt = await eventService.GetByIdAsync(parsedId, cancellationToken);
        return evt is null ? Results.NotFound() : Results.Ok(new GetEventByIdResponse(evt));
    }
}
