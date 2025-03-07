namespace API.Features.IGDB;

public static class Query
{
    public static List<string> Fields =
    [
        "age_ratings.rating_category",
        "age_ratings.organization",
        "artworks.image_id",
        "game_type",
        "cover.image_id",
        "screenshots.image_id",
        "release_dates.*",
        "release_dates.platform.name",
        "game_engines.name",
        "game_modes.name",
        "genres.name",
        "involved_companies.*",
        "involved_companies.company.name",
        "first_release_date",
        "keywords.name",
        "multiplayer_modes",
        "name",
        "platforms.name",
        "player_perspectives.name",
        "rating",
        "release_dates.*",
        "similar_games.name",
        "similar_games.cover.image_id",
        "slug",
        "game_status",
        "storyline",
        "summary",
        "themes.name",
        "url",
        "version_title",
        "websites.*"
    ];
}