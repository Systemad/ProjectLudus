namespace Catalog.Worker.ETL.Pipelines;

public interface IPipeline<T>
{
    public string Name { get; set; }
    public List<IPipelineStep<T>> Steps { get; }
    
    Task<T> ExecuteAsync(T input, CancellationToken cancellationToken);
}
public class Pipeline<T>
{
    private readonly List<IPipelineStep<T>> _steps = new();

    public Pipeline<T> AddStep(IPipelineStep<T> step)
    {
        _steps.Add(step);
        return this;
    }

    public async Task<T> ExecuteAsync(T input, CancellationToken cancellationToken = default)
    {
        T result = input;

        foreach (var step in _steps)
        {
            try
            {
                result = await step.ProcessAsync(result, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return result;
    }
}
