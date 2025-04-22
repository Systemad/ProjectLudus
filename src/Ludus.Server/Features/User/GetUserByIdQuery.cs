using System.Linq.Expressions;
using Marten.Linq;

namespace Ludus.Server.Features.User;

public class GetUserByIdQuery : ICompiledQuery<User, User>
{
    public Guid Id { get; set; }

    public Expression<Func<IMartenQueryable<Features.User.User>, Features.User.User>> QueryIs()
    {
        return q => q.FirstOrDefault(x => x.Id == Id);
    }
}
