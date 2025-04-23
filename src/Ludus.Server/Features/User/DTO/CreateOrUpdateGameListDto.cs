namespace Ludus.Server.Features.User.DTO;

public class CreateOrUpdateGameListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; }
}
