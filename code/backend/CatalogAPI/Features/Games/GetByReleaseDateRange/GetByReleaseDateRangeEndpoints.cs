using CatalogAPI.Context;
using CatalogAPI.Data;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

// TODO
/*
 * Only use Typesense for /searchsense
 * Generate hooks with Kubb
 * FRONTEND: Use this endpoint for upcoming games, add the ability in the frontend to choose this week, month, next month
 * FRONTEND: Add the ability to browse games release last week, last month
 * FRONTEND: Use PopularityTypes endpoints, to get "Steam Most Wishlisted", "Most Watched" etc
 */
namespace CatalogAPI.Features.Games.GetByReleaseDateRange;

public static class GetByReleaseDateRangeEndpoints
{
    private record GamesReleaseDateRangeResponse(
        long From,
        long To,
        int Limit,
        List<GamesSearch> Games
    );

    public static IEndpointRouteBuilder MapGetByReleaseDateRangeEndpoints(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        var group = routeBuilder.MapGroup("/api/games");

        group
            .MapGet("/release-date-range", GetGamesByReleaseDateRangeAsync)
            .Produces<GamesReleaseDateRangeResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return routeBuilder;
    }

    private static async Task<IResult> GetGamesByReleaseDateRangeAsync(
        [AsParameters] GetByReleaseDateRangeQuery query,
        IValidator<GetByReleaseDateRangeQuery> validator,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        ValidationResult validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var games = await db
            .GamesSearches.Where(g => g.FirstReleaseDate.HasValue)
            .Where(g =>
                g.FirstReleaseDate!.Value >= query.From && g.FirstReleaseDate.Value <= query.To
            )
            .OrderByDescending(g => g.FirstReleaseDate)
            .Take(query.Limit)
            .ToListAsync(cancellationToken);

        return Results.Ok(
            new GamesReleaseDateRangeResponse(query.From, query.To, query.Limit, games)
        );
    }
}
