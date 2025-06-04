using System.Linq.Expressions;
using Marten.Linq;

namespace Ludus.Server.Features.Common.Quries;

public class FindCollectionByUser : ICompiledQuery<UserGameState, UserGameState>
{
    public Guid UserId { get; set; }
    public long GameId { get; set; }

    public Expression<Func<IMartenQueryable<UserGameState>, UserGameState>> QueryIs()
    {
        return q => q.FirstOrDefault(x => x.GameId == GameId && x.UserId == UserId);
    }
}
