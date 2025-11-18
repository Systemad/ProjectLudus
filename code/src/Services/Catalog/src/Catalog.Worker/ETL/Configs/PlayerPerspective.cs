using NpgsqlTypes;

namespace Catalog.Worker.ETL.Configs;

public static class PlayerPerspectiveConfig
{
    public static EntityMetadata<IGDB.Models.PlayerPerspective> Config { get; } =
        new(
            Endpoint: "player_perspectives",
            Fields: ["*"],
            CopyCommand: "COPY player_perspectives (id, name, slug, url, updated_at, created_at) FROM STDIN (FORMAT BINARY)",
            WriteRow: (writer, item) =>
            {
                writer.Write(item.Id, NpgsqlDbType.Bigint);
                writer.Write(item.Name, NpgsqlDbType.Text);
                writer.Write(item.Slug, NpgsqlDbType.Text);
                writer.Write(item.Url, NpgsqlDbType.Text);
                writer.Write(item.UpdatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item.CreatedAt, NpgsqlDbType.TimestampTz);
            }
        );
}