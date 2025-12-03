using IGDB.Models;
using NpgsqlTypes;

namespace Catalog.Worker.ETL.Configs;

public static class FranchiseConfig
{
    public static EntityMetadata<Franchise> Config = new(
        Endpoint: "franchises",
        Fields: ["*"],
        CopyCommand: "COPY franchises (id, name, slug, url, updated_at, created_at) FROM STDIN (FORMAT BINARY)",
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
