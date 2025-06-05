namespace Ludus.Server.Features.Me.Lists.Update;

public class UpdateListRequest
{
    public Guid ListId { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; }
}
