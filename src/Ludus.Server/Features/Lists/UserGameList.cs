namespace Ludus.Server.Features.Lists;

public class UserGameList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; } = false;
    public List<long> GamesIds { get; set; } = new();
}
