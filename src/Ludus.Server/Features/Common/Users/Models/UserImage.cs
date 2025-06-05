namespace Ludus.Server.Features.Common.Users.Models;

public class UserImage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public DateTime CreatedDate { get; set; }
}
