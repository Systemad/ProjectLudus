using Npgsql;

namespace Catalog.Worker.ETL;

public record EntityMetadata<T>(
    string Endpoint,
    IReadOnlyList<string> Fields,
    string CopyCommand,
    Action<NpgsqlBinaryImporter, T> WriteRow);
