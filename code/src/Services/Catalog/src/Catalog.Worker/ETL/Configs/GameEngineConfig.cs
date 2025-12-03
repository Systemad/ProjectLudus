using IGDB.Models;
using NpgsqlTypes;

namespace Catalog.Worker.ETL.Configs;

public static class GameEngineConfig
{
    public static EntityMetadata<GameEngine> Config { get; } =
        new(
            Endpoint: "gameengines",
            Fields:
            [
                "name",
                "platforms",
                "logo.id",
                "logo.image_id",
                "logo.url",
                "logo.slug",
                "logo.url",
                "updated_at",
                "created_at"
            ],
            CopyCommand: "COPY game_engines (id, name, slug, url, start_date, updated_at, created_at, metadata) FROM STDIN (FORMAT BINARY)",
            WriteRow: (writer, item) =>
            {
                writer.Write(item.Id, NpgsqlDbType.Bigint);
                writer.Write(item.Name, NpgsqlDbType.Text);
                writer.Write(item.Slug, NpgsqlDbType.Text);
                writer.Write(item.Url, NpgsqlDbType.Text);
                writer.Write(item.UpdatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item.CreatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item, NpgsqlDbType.Jsonb);
                writer.Write(item.Logo, NpgsqlDbType.Jsonb);
            }
        );
}