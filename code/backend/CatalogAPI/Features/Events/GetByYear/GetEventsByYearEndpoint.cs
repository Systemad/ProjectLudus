using CatalogAPI.Features.Events.Dtos;

namespace CatalogAPI.Features.Events.GetByYear;

public static class GetEventsByYearEndpoint
{
    private record EventsYearResponse(int Year, List<EventDto> Events);

    public static RouteHandlerBuilder MapGetEventsByYearEndpoint(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        return routeBuilder
            .MapGet("/year/{year:int}", GetEventsByYearAsync)
            .WithName($"{EndpointMetadata.Events}/GetByYear")
            .WithTags(EndpointMetadata.Events)
            .Produces<EventsYearResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> GetEventsByYearAsync(
        AppDbContext db,
        int year,
        CancellationToken cancellationToken
    )
    {
        if (year is < 1 or > 9999)
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    [nameof(year)] = ["Year must be between 1 and 9999."],
                }
            );
        }

        var yearStart = Instant.FromUtc(year, 1, 1, 0, 0);
        var nextYearStart = yearStart.Plus(
            Duration.FromDays(DateTime.IsLeapYear(year) ? 366 : 365)
        );

        var eventDtos = await db
            .Events.Where(e => e.StartTimeUtc >= yearStart && e.StartTimeUtc < nextYearStart)
            .OrderBy(e => e.StartTimeUtc)
            .Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name ?? string.Empty,
                Slug = e.Slug,
                Description = e.Description,
                LiveStreamUrl = e.LiveStreamUrl,
                StartTimeUtc = e.StartTimeUtc != null ? e.StartTimeUtc.Value.ToDateTimeUtc() : null,
                EndTimeUtc = e.EndTimeUtc != null ? e.EndTimeUtc.Value.ToDateTimeUtc() : null,
                TimeZone = e.TimeZone,
                LogoImageId = e.EventLogoNavigation != null ? e.EventLogoNavigation.ImageId : null,
            })
            .ToListAsync(cancellationToken);

        var eventIds = eventDtos.Select(e => e.Id).ToList();

        var gamesByEventId = await db
            .Events.Where(e => eventIds.Contains(e.Id))
            .SelectMany(e => e.Games.Select(g => new { EventId = e.Id, GameId = g.Id }))
            .Join(
                db.GamesSearches,
                eg => eg.GameId,
                s => s.Id,
                (eg, s) => new { eg.EventId, Search = s }
            )
            .ToListAsync(cancellationToken);

        var grouped = gamesByEventId
            .GroupBy(x => x.EventId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Search).ToList());

        foreach (var dto in eventDtos)
        {
            if (grouped.TryGetValue(dto.Id, out var games))
                dto.Games = GameSearchMapper.MapToDto(games);
        }

        return Results.Ok(new EventsYearResponse(year, eventDtos));
    }
}
