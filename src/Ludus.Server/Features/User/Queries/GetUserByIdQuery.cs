using System.Linq.Expressions;
using Marten.Linq;

namespace Ludus.Server.Features.User.Queries;

public class GetUserByIdQuery : ICompiledQuery<Models.User, Models.User>
{
    public Guid Id { get; set; }

    public Expression<Func<IMartenQueryable<Models.User>, Models.User>> QueryIs()
    {
        return q => q.FirstOrDefault(x => x.Id == Id);
    }
}
