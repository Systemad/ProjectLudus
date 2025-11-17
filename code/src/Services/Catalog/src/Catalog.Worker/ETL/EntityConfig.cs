using IGDB.Models;
using NpgsqlTypes;

namespace Catalog.Worker.ETL;

public static class EntityConfig
{
    public static EntityMetadata<Company> CompanyConfig { get; } =
        new(
            Endpoint: "companies",
            Fields:
            [
                "id",
                "name",
                "slug",
                "url",
                "startDdate",
                "updatedAt",
                "createdAt"
            ],
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
    
    private static readonly Dictionary<Type, object> Configurations = new()
    {
        { typeof(Company), CompanyConfig },
    };
    
    public static EntityMetadata<T> Get<T>()
    {
        if (Configurations.TryGetValue(typeof(T), out var config))
        {
            return (EntityMetadata<T>)config;
        }
        throw new KeyNotFoundException($"Configuration not found for entity type {typeof(T).Name}");
    }
}
