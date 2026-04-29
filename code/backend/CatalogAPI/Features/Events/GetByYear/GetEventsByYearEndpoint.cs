using CatalogAPI.Features.Events.Dtos;
using CatalogAPI.Features.Games.Common.Projections;

namespace CatalogAPI.Features.Events.GetByYear;

public static class GetEventsByYearEndpoint
{
    public record GetEventsByYearResponse(int Year, List<EventDto> Events);

    public static async Task<IResult> HandleAsync(
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
                Games = e
                    .Games.AsQueryable()
                    .Where(g => g.FirstReleaseDateUtc.HasValue)
                    .Select(GameDtoProjection.AsGameDto)
                    .ToList(),
            })
            .ToListAsync(cancellationToken);

        return Results.Ok(new GetEventsByYearResponse(year, eventDtos));
    }
}
