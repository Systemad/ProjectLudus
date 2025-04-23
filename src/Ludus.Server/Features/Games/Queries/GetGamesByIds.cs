using System.Linq.Expressions;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Linq;

namespace Ludus.Server.Features.Games.Queries;

public class GetGamesByIdQuery : ICompiledListQuery<Game>
{
    public GetGamesByIdQuery(List<long> gameIds)
    {
        GameIds = gameIds;
    }

    public List<long> GameIds { get; set; }

    public QueryStatistics Stats { get; } = new QueryStatistics();

    public Expression<Func<IMartenQueryable<Game>, IEnumerable<Game>>> QueryIs()
    {
        return query => query.Where(g => g.Id.IsOneOf(GameIds));
    }
}
