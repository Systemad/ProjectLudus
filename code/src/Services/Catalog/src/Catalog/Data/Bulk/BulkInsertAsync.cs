using Npgsql;

namespace Catalog.Data.Bulk;

public class BulkInsertService
{
    public async Task BulkInsertAsync<T>(List<T> items, NpgsqlConnection connection, IBulkInsert<T> mapper) where T : class
    {
        await using var writer = connection.BeginBinaryImport(mapper.CopyCommand);

        foreach (var item in items)
        {
            writer.StartRow();
            mapper.WriteRow(writer, item);
        }

        await writer.CompleteAsync();
        await writer.CloseAsync();
    }
    
    public async Task BulkInsertAsync2<T>(IEnumerable<T> items, NpgsqlConnection connection, string copyCommand, Action<NpgsqlBinaryImporter, T> writeRow)
    {
        await using var writer = connection.BeginBinaryImport(copyCommand);

        foreach (var item in items)
        {
            writer.StartRow();
            writeRow(writer, item);
        }

        await writer.CompleteAsync();
        await writer.CloseAsync();
    }
}