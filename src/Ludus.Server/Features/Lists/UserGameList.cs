namespace Ludus.Server.Features.Lists;

public class UserGameList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; } = false;
    public List<Guid> GameEntryIds { get; set; } = new();
}

public record UserGameListDto(
    Guid Id,
    string Name,
    bool Public,
    IEnumerable<GameEntryPreviewDto> GameEntries
);
