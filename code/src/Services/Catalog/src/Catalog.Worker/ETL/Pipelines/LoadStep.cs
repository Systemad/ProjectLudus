using Npgsql;
using Parquet.Serialization;

namespace Catalog.Worker.ETL.Pipelines;

public class LoadStep<T> : IPipelineStep<EntityMetadata<T>> where T : class, new()
{
    // TODO: check this again?
    private readonly NpgsqlConnection _connection;
    public async Task<EntityMetadata<T>> ProcessAsync(EntityMetadata<T> input, CancellationToken cancellationToken)
    {
        var file = $"Data/{input.Endpoint}.parquet";
        var items = await ParquetSerializer.DeserializeAsync<T>(file, cancellationToken: cancellationToken);
        
        await using var writer = await _connection.BeginBinaryImportAsync(input.CopyCommand, cancellationToken);
        foreach (var item in items)
        {
            writer.StartRow();
            input.WriteRow(writer, item);
        }
        await writer.CompleteAsync(cancellationToken);
        await writer.CloseAsync(cancellationToken);

        return input;
    }
    
    
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