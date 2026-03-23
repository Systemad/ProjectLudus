using System.Text.Json;
using CatalogAPI.Features.Tags;
using CatalogAPI.Models;

namespace CatalogAPI.Features.Search;

public static class FacetGroupBuilder
{
    private static readonly AggregationBuckets EmptyBuckets = new()
    {
        Buckets = [],
        SumOtherDocCount = 0,
    };

    public static List<FacetGroupResponse> Build(
        SearchRequest req,
        SearchAggregateResult? aggregates
    ) =>
    [
        new(
            Key: TagKeys.GENRES,
            Label: "Genres",
            Order: 1,
            MultiSelect: true,
            Selected: GetSelected(req.Genres),
            Buckets: ParseBuckets(aggregates?.Genres)
        ),

        new(
            Key: TagKeys.THEMES,
            Label: "Themes",
            Order: 2,
            MultiSelect: true,
            Selected: GetSelected(req.Themes),
            Buckets: ParseBuckets(aggregates?.Themes)
        ),

        new(
            Key: TagKeys.GAME_MODES,
            Label: "Game modes",
            Order: 3,
            MultiSelect: true,
            Selected: GetSelected(req.GameModes),
            Buckets: ParseBuckets(aggregates?.GameModes)
        ),

        new(
            Key: TagKeys.MULTIPLAYER_MODES,
            Label: "Multiplayer",
            Order: 4,
            MultiSelect: true,
            Selected: GetSelected(req.Multiplayer),
            Buckets: ParseBuckets(aggregates?.MultiPlayerModes)
        ),

        new(
            Key: TagKeys.PLAYER_PERSPECTIVE,
            Label: "Player perspectives",
            Order: 5,
            MultiSelect: true,
            Selected: GetSelected(req.Perspectives),
            Buckets: ParseBuckets(aggregates?.PlayerPerspective)
        )

    ];

    private static List<string> GetSelected(string[]? values)
    {
        if (values is not { Length: > 0 })
        {
            return new List<string>();
        }

        return values
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => v.ToLower())
            .Distinct()
            .ToList();
    }

    private static AggregationBuckets ParseBuckets(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return EmptyBuckets;
        }

        return JsonSerializer.Deserialize<AggregationBuckets>(value) ?? EmptyBuckets;
    }
}
