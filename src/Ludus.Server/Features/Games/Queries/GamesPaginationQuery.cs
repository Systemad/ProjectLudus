using System.Linq.Expressions;
using Ludus.Shared.Features.Games;
using Marten.Linq;

namespace Ludus.Server.Features.Games.Queries;

public class GamesPaginationQuery : ICompiledListQuery<Game>
{
    public GamesPaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public QueryStatistics Stats { get; } = new QueryStatistics();

    public Expression<Func<IMartenQueryable<Game>, IEnumerable<Game>>> QueryIs() =>
        query => query.Skip(PageNumber).Take(PageSize).OrderByDescending(g => g.RatingCount);
}
