using Catalog.Worker.Services;
using Npgsql;
using Parquet;
using Parquet.Serialization;

namespace Catalog.Worker.ETL;

public class EtlService(IgdbService service)
{
    public async Task FetchDataAsync<T>(CancellationToken cancellationToken, EntityMetadata<T> config)
    {
        long amount = 0;
        await foreach (
            var page in service.FetchAllPagesAsync<T>(config.Endpoint, config.Fields).WithCancellation(cancellationToken)
        )
        {
            var append = amount > 0;
            var file = $"Data/{config.Endpoint}.parquet";
            await ParquetSerializer.SerializeAsync(
                page.Items,
                file,
                new ParquetSerializerOptions { Append = append },
                cancellationToken: cancellationToken
            );
            amount += page.Items.Count;
            if (amount >= page.TotalCount)
                break;
        }
    }
    
    // TODO: not sure if this will work
    public async Task WriteDataAsync<T>(NpgsqlConnection connection, EntityMetadata<T> config, CancellationToken cancellationToken) where T : class, new()
    {
        var file = $"Data/{config.Endpoint}.parquet";
            var items = await ParquetSerializer.DeserializeAsync<T>(file, cancellationToken: cancellationToken);
        
        await using var writer = await connection.BeginBinaryImportAsync(config.CopyCommand, cancellationToken);
        foreach (var item in items)
        {
            writer.StartRow();
            config.WriteRow(writer, item);
        }
        await writer.CompleteAsync(cancellationToken);
        await writer.CloseAsync(cancellationToken);
    }
}