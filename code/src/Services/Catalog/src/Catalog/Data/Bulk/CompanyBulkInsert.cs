using IGDB.Models;
using Npgsql;
using NpgsqlTypes;

namespace Catalog.Data.Bulk;

public class CompanyBulkInsert : IBulkInsert<Company>
{
    public string CopyCommand =>
        "COPY companies (id, name, slug, url, start_date, updated_at, created_at, metadata) FROM STDIN (FORMAT BINARY)";

    public void WriteRow(NpgsqlBinaryImporter writer, Company item)
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
}

public static class CompanyBulkInsert2
{
    public static string CopyCommand =
        "COPY companies (id, name, slug, url, start_date, updated_at, created_at, metadata) FROM STDIN (FORMAT BINARY)";

    private static void WriteRow(NpgsqlBinaryImporter writer, Company item)
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
    
    public static Action<NpgsqlBinaryImporter, Company> writeRowDelegate = WriteRow;
}

