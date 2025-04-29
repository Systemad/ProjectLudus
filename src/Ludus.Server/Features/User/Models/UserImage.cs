namespace Ludus.Server.Features.User.Models;

public class UserImage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public DateTime CreateDate { get; set; }
}
