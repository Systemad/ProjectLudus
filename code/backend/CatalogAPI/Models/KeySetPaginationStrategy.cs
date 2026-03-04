using Jameak.CursorPagination.Abstractions.Attributes;
using Jameak.CursorPagination.Abstractions.Enums;

namespace CatalogAPI.Models;

[KeySetPaginationStrategy(typeof(GameSearchFacet), CursorSerialization: KeySetCursorSerializerGeneration.UseSystemTextJson)]
[PaginationProperty(Order: 0, nameof(GameSearchFacet.Score), PaginationOrdering.Descending)]
[PaginationProperty(Order: 1, nameof(GameSearchFacet.Id), PaginationOrdering.Descending)]
public partial class KeySetPaginationStrategy
{
    
}
//

//[PaginationProperty(Order: 1, nameof(GamesSearch.FirstReleaseDate), PaginationOrdering.Descending)]
