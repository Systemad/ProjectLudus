using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.User.List;

public class UserGameListDto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    //public string UserId { get; set; }

    public string Name { get; set; }
    public bool Public { get; set; }
    public List<Game>? Games { get; set; }
    public int Items => Games?.Count ?? 0;
}
