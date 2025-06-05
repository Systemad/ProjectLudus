namespace Ludus.Server.Features.Common.Endpoints;

public interface IPaginatedResponse<T>
{
    IEnumerable<T> Items { get; }
    public long TotalItemCount { get; }
    public long PageCount { get; }
    public long PageNumer { get; }

    public bool IsLastPage { get; }
}
