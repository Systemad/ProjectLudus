using Ludus.Server.Features.Common.Collection;

namespace Me.Lists.Item.Add;

public class AddGameToListRequest : IGameIdRequest
{
    public Guid ListId { get; set; }
    public long GameId { get; set; }
}
