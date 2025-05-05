namespace Ludus.Server.Features.Shared;

public interface IPaginatedResponse
{
    public long TotalItemCount { get; }
    public long PageCount { get; }
    public bool IsLastPage { get; }
}
