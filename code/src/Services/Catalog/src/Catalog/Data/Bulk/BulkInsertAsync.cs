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
    }
}