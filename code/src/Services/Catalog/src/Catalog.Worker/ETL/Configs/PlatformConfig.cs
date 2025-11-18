using IGDB.Models;
using NpgsqlTypes;

namespace Catalog.Worker.ETL.Configs;

public static class PlatformConfig
{
    public static EntityMetadata<Platform> Config { get; } =
        new(
            Endpoint: "platforms",
            Fields:
            [
                "abbreviation",
                "alternative_name",
                "checksum",
                "created_at",
                "generation",
                "name",
                "platform_family",
                "platform_logo.*",
                "platform_type.*",
                "slug",
                "summary",
                "updated_at",
                "url",
                "versions",
                "websites",
            ],
            CopyCommand: "COPY platforms (id, name, abbreviation, generation, slug, url, updated_at, created_at, logo) FROM STDIN (FORMAT BINARY)",
            WriteRow: (writer, item) =>
            {
                writer.Write(item.Id, NpgsqlDbType.Bigint);
                writer.Write(item.Name, NpgsqlDbType.Text);
                writer.Write(item.Abbreviation, NpgsqlDbType.Text);
                writer.Write(item.Generation, NpgsqlDbType.Bigint);
                writer.Write(item.Slug, NpgsqlDbType.Text);
                writer.Write(item.Url, NpgsqlDbType.Text);
                writer.Write(item.UpdatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item.CreatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item.PlatformLogo, NpgsqlDbType.Jsonb);
            }
        );
}