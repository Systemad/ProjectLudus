namespace Me.Lists;

public class GameListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; }
    public int TotalItems { get; set; }
    public DateTimeOffset Created { get; set; }
    public List<GameListItemDto> Items { get; set; }
}
