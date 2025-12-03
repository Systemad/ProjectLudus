using IGDB.Models;
using NpgsqlTypes;

namespace Catalog.Worker.ETL.Configs;

public static class CompanyConfig
{
    public static EntityMetadata<Company> Config { get; } =
        new(
            Endpoint: "companies",
            Fields: ["*", "status.name", "websites.type.type", "websites.*", "logo.*;"],
            CopyCommand: "COPY companies (id, name, slug, url, start_date, updated_at, created_at, metadata) FROM STDIN (FORMAT BINARY)",
            WriteRow: (writer, item) =>
            {
                writer.Write(item.Id, NpgsqlDbType.Bigint);
                writer.Write(item.Name, NpgsqlDbType.Text);
                writer.Write(item.Slug, NpgsqlDbType.Text);
                writer.Write(item.Url, NpgsqlDbType.Text);
                writer.Write(item.StartDate, NpgsqlDbType.TimestampTz);
                writer.Write(item.UpdatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item.CreatedAt, NpgsqlDbType.TimestampTz);
                writer.Write(item, NpgsqlDbType.Jsonb);
            }
        );
}
