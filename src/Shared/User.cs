namespace Shared;

public class LudusUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string SteamId { get; set; }
    public int? AvatarImageId { get; set; }
    public string Color { get; set; }
    public DateTime CreateDate { get; set; }

    public string AvatarUrl() => AvatarImageId.HasValue ? 
        $"api/images/{AvatarImageId}" : "/img/profiles/default_avatar.png";
    
    //public int LudusUserImageId { get; set; }
    public LudusUserImage? LudusUserImage { get; set; }
}

public class LudusUserImage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
    public int? UserId { get; set; }
    public DateTime CreateDate { get; set; }

    public int LudusUserId { get; set; }
    public LudusUser LudusUser { get; set; } = null!;
}