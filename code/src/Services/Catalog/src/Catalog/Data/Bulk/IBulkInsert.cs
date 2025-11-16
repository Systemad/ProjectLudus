using Npgsql;

namespace Catalog.Data.Bulk;

public interface IBulkInsert<T> where T : class
{
    string CopyCommand { get; }
    void WriteRow(NpgsqlBinaryImporter writer, T item);
}