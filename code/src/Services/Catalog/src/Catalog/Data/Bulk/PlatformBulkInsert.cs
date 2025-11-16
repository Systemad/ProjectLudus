using IGDB.Models;
using Npgsql;
using NpgsqlTypes;

namespace Catalog.Data.Bulk;

public class PlatformBulkInsert : IBulkInsert<Platform>
{
    public string CopyCommand =>
        "COPY companies (id, name, abbreviation, generation, slug, url, updated_at, created_at, logo) FROM STDIN (FORMAT BINARY)";

    public void WriteRow(NpgsqlBinaryImporter writer, Platform item)
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
}
