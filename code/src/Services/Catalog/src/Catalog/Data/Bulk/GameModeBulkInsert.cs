using IGDB.Models;
using Npgsql;
using NpgsqlTypes;

namespace Catalog.Data.Bulk;

public class GameModeBulkInsert : IBulkInsert<GameMode>
{
    public string CopyCommand =>
        "COPY companies (id, name, slug, url, updated_at, created_at) FROM STDIN (FORMAT BINARY)";

    public void WriteRow(NpgsqlBinaryImporter writer, GameMode item)
    {
        writer.Write(item.Id, NpgsqlDbType.Bigint);
        writer.Write(item.Name, NpgsqlDbType.Text);
        writer.Write(item.Slug, NpgsqlDbType.Text);
        writer.Write(item.Url, NpgsqlDbType.Text);
        writer.Write(item.UpdatedAt, NpgsqlDbType.TimestampTz);
        writer.Write(item.CreatedAt, NpgsqlDbType.TimestampTz);
        writer.Write(item, NpgsqlDbType.Jsonb);
    }
}
