using CatalogAPI.Features.Events.Dtos;
using CatalogAPI.Features.Games.Common.Projections;
using CatalogAPI.Features.Games.GetMedia;

namespace CatalogAPI.Features.Events.GetById;

public static class GetEventByIdEndpoint
{
    private record EventDetailResponse(EventDto Event);

    public static RouteHandlerBuilder MapGetEventByIdEndpoint(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        return routeBuilder
            .MapGet("/{id:long}", GetEventByIdAsync)
            .WithName($"{EndpointMetadata.Events}/GetById")
            .WithTags(EndpointMetadata.Events)
            .Produces<EventDetailResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetEventByIdAsync(
        long id,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var dto = await db
            .Events.Where(e => e.Id == id)
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
                Videos = e
                    .Videos.Select(v => new GameMediaVideoDto(
                        v.Name ?? string.Empty,
                        v.VideoId ?? string.Empty
                    ))
                    .ToList(),
                Games = e
                    .Games.Where(g => g.FirstReleaseDateUtc.HasValue)
                    .AsQueryable()
                    .Select(GameDtoProjection.AsGameDto)
                    .ToList(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
            return Results.NotFound();

        return Results.Ok(new EventDetailResponse(dto));
    }
}
