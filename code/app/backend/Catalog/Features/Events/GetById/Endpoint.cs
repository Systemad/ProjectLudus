namespace Catalog.Features.Events.GetById;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetEventByIdResponse>>> HandleAsync(
        string id,
        IEventService eventService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(id, out var parsedId))
            return TypedResults.BadRequest();

        var evt = await eventService.GetByIdAsync(parsedId, cancellationToken);
        return evt is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new GetEventByIdResponse(evt));
    }
}
