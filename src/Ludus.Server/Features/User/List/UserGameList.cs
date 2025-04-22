namespace Ludus.Server.Features.User.List;

public class UserGameList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; } = false;
    public List<long> GamesIds { get; set; }
}
