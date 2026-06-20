namespace CatalogAPI.Features.Events.GetById;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long id,
        IEventService eventService,
        CancellationToken cancellationToken
    )
    {
        var evt = await eventService.GetByIdAsync(id, cancellationToken);
        return evt is null ? Results.NotFound() : Results.Ok(new GetEventByIdResponse(evt));
    }
}
