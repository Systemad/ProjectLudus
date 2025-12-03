namespace Catalog.Worker.ETL.Pipelines;

public interface IPipelineStep<T>
{
    Task<T> ProcessAsync(T input, CancellationToken cancellationToken);
}