using WebAPI.Features.Common.Games.Models;

namespace Me.Lists;

public class GameListItemDto
{
    public Guid Id { get; set; }
    public DateTimeOffset AddedAt { get; set; }
    public GameDto Game { get; set; }
    public Guid GameListId { get; set; }
}
