namespace WebAPI.Features.Common.Users.DTO;



public class UserImageDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public DateTime CreatedDate { get; set; }
    //public Guid UserId { get; set; }
}