using Ludus.Server.Features.User.List;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.User.DTO;

public class UserGameListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; }
    public List<Game>? Games { get; set; }
    public int Count { get; set; }
}

public static class UserGameListMapping
{
    public static UserGameListDto AsUserGameListDto(this UserGameList item)
    {
        return new UserGameListDto
        {
            Id = item.Id,
            Name = item.Name,
            Public = item.Public,
        };
    }
}
