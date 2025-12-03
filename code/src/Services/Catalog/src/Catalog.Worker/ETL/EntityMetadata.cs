using Npgsql;

namespace Catalog.Worker.ETL;

public interface IEntityMetadata
{
    string Endpoint { get; }
    IReadOnlyList<string> Fields { get; }
    string CopyCommand { get; }
    void WriteRow(NpgsqlBinaryImporter writer, object item);
}

public record EntityMetadata<T>(
    string Endpoint,
    IReadOnlyList<string> Fields,
    string CopyCommand,
    Action<NpgsqlBinaryImporter, T> WriteRow) : IEntityMetadata
{
    void IEntityMetadata.WriteRow(NpgsqlBinaryImporter writer, object item)
    {
        WriteRow(writer, (T)item);
    }
}
