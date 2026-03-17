namespace CatalogAPI.Features.Search;



//[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortOption
{
    Relevancy,
    //ReleaseDate,
    //Rating
}

//public SortOption? SortOption { get; set; } = Models.SortOption.Relevancy;
    
// public long? Cursor { get; init; }