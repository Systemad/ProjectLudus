namespace WebAPI.Features.Common.Users.Models;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public UserImage UserImage { get; set; }
}
