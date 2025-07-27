namespace WebAPI.Features.Common.Users.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public UserImageDto? UserImage { get; set; }
}
