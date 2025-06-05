namespace Ludus.Server.Features.Me.Lists.Item.Add;

public class AddGameRequest
{
    public Guid ListId { get; set; }
    public long GameId { get; set; }
}
