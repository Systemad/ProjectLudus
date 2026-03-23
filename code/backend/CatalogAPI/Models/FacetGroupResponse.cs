namespace CatalogAPI.Models;

public record FacetGroupResponse(
    string Key,
    string Label,
    int Order,
    bool MultiSelect,
    List<string> Selected,
    AggregationBuckets Buckets
);
