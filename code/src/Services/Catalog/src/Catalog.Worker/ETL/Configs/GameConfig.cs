using IGDB.Models;
using NpgsqlTypes;

namespace Catalog.Worker.ETL.Configs;

public static class GameConfig
{
    public static EntityMetadata<Game> Config { get; } =
        new(
            Endpoint: "games",
            Fields:
            [
                "age_ratings.content_descriptions.description",
                "age_ratings.organization.name",
                "age_ratings.rating_category.rating",
                "age_ratings.rating_content_descriptions.description",
                "age_ratings.rating_cover_url",
                "alternative_names.name",
                "artworks.animated",
                "artworks.image_id",
                "artworks.url",
                "checksum",
                "collections.name",
                "collections.slug",
                "collections.url",
                "cover.animated",
                "cover.image_id",
                "cover.url",
                "created_at",
                "dlcs",
                "expanded_games",
                "expansions",
                "first_release_date",
                "franchises",
                "game_engines",
                "game_modes",
                "game_status.status",
                "game_type",
                "genres",
                "involved_companies.company",
                "involved_companies.developer",
                "involved_companies.porting",
                "involved_companies.publisher",
                "involved_companies.supporting",
                "keywords.name",
                "keywords.slug",
                "keywords.url",
                "language_supports.language.name",
                "language_supports.language.locale",
                "language_supports.language_support_type.name",
                "multiplayer_modes.campaigncoop",
                "multiplayer_modes.lancoop",
                "multiplayer_modes.offlinecoop",
                "multiplayer_modes.offlinecoopmax",
                "multiplayer_modes.offlinemax",
                "multiplayer_modes.onlinecoop",
                "multiplayer_modes.onlinecoopmax",
                "multiplayer_modes.onlinemax",
                "multiplayer_modes.splitscreen",
                "multiplayer_modes.splitscreenonline",
                "name",
                "platforms",
                "hypes",
                "rating",
                "rating_count",
                "release_dates.date",
                "release_dates.date_format",
                "release_dates.human",
                "release_dates.status",
                "release_dates.y",
                "release_dates.release_region.region",
                "release_dates.platform.name",
                "screenshots.animated",
                "screenshots.url",
                "screenshots.image_id",
                "similar_games",
                "standalone_expansions.id",
                "slug",
                "storyline",
                "summary",
                "themes",
                "total_rating",
                "total_rating_count",
                "updated_at",
                "url",
                "version_title",
                "videos.name",
                "videos.video_id",
                "websites.type.type",
                "websites.url",
            ],
            CopyCommand: "COPY games (id, name, slug, game_type, rating, rating_count, total_rating, total_rating_count, url, first_release_date, updated_at, created_at, metadata) FROM STDIN (FORMAT BINARY)",
            WriteRow: (writer, item) =>
            {
                writer.Write(item.Id, NpgsqlDbType.Bigint);
                writer.Write(item.Name, NpgsqlDbType.Text);
                writer.Write(item.Slug, NpgsqlDbType.Text);
                writer.Write(item.GameType.Value.Id, NpgsqlDbType.Bigint);
                writer.Write(item.Rating, NpgsqlDbType.Double);
                writer.Write(item.RatingCount, NpgsqlDbType.Bigint);
                writer.Write(item.TotalRating, NpgsqlDbType.Double);
                writer.Write(item.TotalRatingCount, NpgsqlDbType.Bigint);
                writer.Write(item.Url, NpgsqlDbType.Text);
                writer.Write(item.FirstReleaseDate, NpgsqlDbType.TimestampTz);
                writer.Write(item.UpdatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item.CreatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item, NpgsqlDbType.Jsonb);
            }
        );
}
