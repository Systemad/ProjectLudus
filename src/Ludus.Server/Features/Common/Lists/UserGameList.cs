using Ludus.Server.Features.Common.Games.Models;

namespace Ludus.Server.Features.Common.Lists;

public class UserGameList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; } = false;
    public List<long> Games { get; set; } = new();
}

public record UserGameListDto(
    Guid Id,
    string Name,
    bool Public,
    int TotalItems,
    IEnumerable<GameDto> Items
);
