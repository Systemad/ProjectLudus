using CatalogAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Tags;

public record TagResponse(List<FullTag> Tags);

public static class TagsEndpoints
{
    private const string GetResource = "GetTags";

    public static IEndpointRouteBuilder UseTagsEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/tags");

        group.MapGet(
            "all-tags",
            static async ([FromServices] AppDbContext context, CancellationToken token) =>
            {
                // null handling???
                var genres = await context
                    .Genres.Select(x => new FullTag(
                        Id: $"{x.Id}:{x.Name}",
                        Name: x.Name,
                        GroupName: TagKeys.GENRES,
                        Slug: x.Slug
                    ))
                    .ToListAsync(cancellationToken: token);
                var themes = await context
                    .Themes.Select(x => new FullTag(
                        Id: $"{x.Id}:{x.Name}",
                        Name: x.Name,
                        GroupName: TagKeys.THEMES,
                        Slug: x.Slug
                    ))
                    .ToListAsync(cancellationToken: token);
                var gameModes = await context
                    .GameModes.Select(x => new FullTag(
                        Id: $"{x.Id}:{x.Name}",
                        Name: x.Name,
                        GroupName: TagKeys.GAME_MODES,
                        Slug: x.Slug
                    ))
                    .ToListAsync(cancellationToken: token);
                /*
                var playerPerspective = await context
                    .PlayerPerspectives.Select(x => new FullTag(
                        Id: $"{x.Id}:{x.Name}",
                        Name: x.Name,
                        GroupName: TagKeys.MODES,
                        Slug: x.Slug
                    ))
                    .ToListAsync(cancellationToken: token);
                
                */
                var tags = genres.Concat(themes).Concat(gameModes).ToList();

                return new TagResponse(tags);
            }
        ).CacheOutput("Expire15Min");
        return group;
    }
}
