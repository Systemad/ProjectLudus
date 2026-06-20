using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Features.Events.GetList;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        IEventService eventService,
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] string? status,
        [FromQuery] int? limit,
        CancellationToken cancellationToken
    )
    {
        if (year is < 1 or > 9999)
            return Results.ValidationProblem(
                new Dictionary<string, string[]> { [nameof(year)] = ["Year must be between 1 and 9999."] });

        if (month is < 1 or > 12)
            return Results.ValidationProblem(
                new Dictionary<string, string[]> { [nameof(month)] = ["Month must be between 1 and 12."] });

        var events = await eventService.GetListAsync(year, month, status, limit, cancellationToken);
        return Results.Ok(new GetEventsListResponse(events));
    }
}
