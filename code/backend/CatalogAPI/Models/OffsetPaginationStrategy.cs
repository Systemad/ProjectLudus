using CatalogAPI.Data;
using Jameak.CursorPagination.Abstractions.Attributes;
using Jameak.CursorPagination.Abstractions.Enums;

namespace CatalogAPI.Models;

/*
 pdb.score(g0."Id") DESC, COALESCE(g0."Id", 0). its adding with the second order, which results in breakage for paradedb scanning
 * This works!
 * SELECT COALESCE(g0."Id"), g0."Name", g0."Summary", g0."Storyline", g0."FirstReleaseDate", g0."GameType", g0."CoverUrl", g0."ReleaseYear", CAST(COUNT (*) OVER () AS bigint), pdb.score(g0."Id"), g0."Themes", g0."Genres", g0."Modes", agg ('{"terms":{"field": "themes"}}') OVER (), agg ('{"terms":{"field": "genres"}}') OVER ()
FROM (
         SELECT g.id AS "Id", g.name AS "Name", g.summary AS "Summary", g.storyline AS "Storyline", g.first_release_date AS "FirstReleaseDate", g.game_type AS "GameType", g.cover_url AS "CoverUrl", g.themes AS "Themes", g.genres AS "Genres", g.modes AS "Modes", g.release_year AS "ReleaseYear"
         FROM games_search AS g
     ) AS g0
ORDER BY pdb.score(g0."Id") DESC, g0."Id"
LIMIT 20
 */
// Configure Offset source generation
[OffsetPaginationStrategy(typeof(GamesSearch))]
[PaginationProperty(Order: 1, nameof(GamesSearch.Id), PaginationOrdering.Ascending)]
public partial class OffsetPaginationStrategy
{
    
}

// Define the columns and their sort order. Supports composite ordering and mixing ascending/descending
//[PaginationProperty(Order: 0, nameof(GamesSearch.Id), PaginationOrdering.Descending)]
//[PaginationProperty(Order: 1, nameof(GamesSearch.Id), PaginationOrdering.Ascending)]