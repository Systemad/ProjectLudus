using Catalog.Worker.Services;
using Parquet.Serialization;

namespace Catalog.Worker.ETL.Pipelines;

public class ExtractStep<T> : IPipelineStep<EntityMetadata<T>>
{
    private IgdbService _igdbService;

    public ExtractStep(IgdbService igdbService)
    {
        _igdbService = igdbService;
    }

    public async Task<EntityMetadata<T>> ProcessAsync(
        EntityMetadata<T> input,
        CancellationToken cancellationToken
    )
    {
        long amount = 0;
        await foreach (
            var page in _igdbService
                .FetchAllPagesAsync<T>(input.Endpoint, input.Fields)
                .WithCancellation(cancellationToken)
        )
        {
            var append = amount > 0;
            var file = $"Data/{input.Endpoint}.parquet";
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
        return input;
    }
}
