namespace Shared;

public class MImage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public int? UserId { get; set; }
    public DateTime CreateDate { get; set; }
}